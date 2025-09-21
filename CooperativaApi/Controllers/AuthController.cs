using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CooperativaApi.Data;
using CooperativaApi.Models;
using CooperativaApi.DTOs;

namespace CooperativaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IConfiguration _config;
        private readonly IPasswordHasher<User> _hasher;

        public AuthController(AppDbContext db, IConfiguration config, IPasswordHasher<User> hasher)
        {
            _db = db;
            _config = config;
            _hasher = hasher;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Devuelve errores de validación claros

            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email ya registrado" });

            var user = new User { Nombre = dto.Nombre, Email = dto.Email, Rol = dto.Rol ?? "Socio" };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            if (user.Rol == "Socio")
            {
                var socio = new Socio
                {
                    UserId = user.Id,
                    NombreCompleto = dto.Nombre,
                    CI = dto.CI,
                    Direccion = dto.Direccion,
                    Telefono = dto.Telefono,
                    ProfesionId = dto.ProfesionId
                };
                _db.Socios.Add(socio);
                await _db.SaveChangesAsync();
            }

            return CreatedAtAction(null, new { id = user.Id });
        }




        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var result = _hasher.VerifyHashedPassword(user, user.PasswordHash, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized(new { message = "Credenciales inválidas" });

            var token = GenerateJwtToken(user);

            // Tiempo de expiración en segundos
            var expiresInSeconds = 3600; // 1 hora
            var response = new LoginResponseDto
            {
                Success = true,
                Data = new LoginDataDto
                {
                    Token = token,
                    TokenType = "Bearer",
                    Expires = expiresInSeconds // <-- aquí ponemos 3600 en lugar de fecha
                }
            };

            return Ok(response);
        }




        private string GenerateJwtToken(User user)
        {
            var jwt = _config.GetSection("Jwt");
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Nombre),
                new Claim(ClaimTypes.Role, user.Rol)
            };

            var expiry = DateTime.UtcNow.AddMinutes(double.Parse(jwt["ExpiryMinutes"] ?? "60"));

            var token = new JwtSecurityToken(
                issuer: jwt["Issuer"],
                audience: jwt["Audience"],
                claims: claims,
                expires: expiry,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [HttpPost("register-admin")]
        public async Task<IActionResult> RegisterAdmin([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
             
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { status = false, message = "Email ya registrado" });

            var user = new User
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Rol = "Admin" // Forzar rol Admin
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return Ok(new { status = true, userId = user.Id, email = user.Email });
        }
        
        [HttpPost("create-admin")]
        public async Task<IActionResult> CreateAdmin([FromBody] CreateUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { message = "Email ya registrado" });

            var admin = new User
            {
                Nombre = dto.Nombre,
                Email = dto.Email,
                Rol = "Admin"
            };

            admin.PasswordHash = _hasher.HashPassword(admin, dto.Password);

            _db.Users.Add(admin);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Admin creado correctamente", adminId = admin.Id });
        }


    }



    // DTOs
    //public record RegisterDto(string Nombre, string Email, string Password, string? Rol);
    public record LoginDto(string Email, string Password);
}
