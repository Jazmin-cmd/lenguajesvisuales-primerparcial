using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CooperativaApi.Data;
using CooperativaApi.DTOs;
using CooperativaApi.Models;
using Microsoft.AspNetCore.Identity;

namespace CooperativaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SociosController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IPasswordHasher<User> _hasher;

        public SociosController(AppDbContext db, IPasswordHasher<User> hasher)
        {
            _db = db;
            _hasher = hasher;
        }

        // GET: api/socios?page=1&limit=5&rol=Socio
        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int limit = 10,
            [FromQuery] string? rol = null)
        {
            var query = _db.Socios
                .Include(s => s.Profesion)
                .Include(s => s.User) // para poder filtrar por el rol del usuario
                .AsQueryable();

            // Filtro por rol, si está definido
            if (!string.IsNullOrEmpty(rol))
            {
                query = query.Where(s => s.User != null && s.User.Rol == rol);
            }

            // Total de registros antes de paginar
            var total = await query.CountAsync();

            // Paginación
            var socios = await query
                .Skip((page - 1) * limit)
                .Take(limit)
                .Select(s => new SocioDto
                {
                    Id = s.Id,
                    NombreCompleto = s.NombreCompleto,
                    CI = s.CI,
                    Direccion = s.Direccion,
                    Telefono = s.Telefono,
                    ProfesionNombre = s.Profesion.Nombre
                })
                .ToListAsync();

            return Ok(new
            {
                total,
                page,
                limit,
                data = socios
            });
        }

        // GET: api/socios/1
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var s = await _db.Socios
                .Include(s => s.Profesion)
                .Where(s => s.Id == id)
                .Select(s => new SocioDto
                {
                    Id = s.Id,
                    NombreCompleto = s.NombreCompleto,
                    CI = s.CI,
                    Direccion = s.Direccion,
                    Telefono = s.Telefono,
                    ProfesionNombre = s.Profesion.Nombre
                })
                .FirstOrDefaultAsync();

            if (s == null) return NotFound();
            return Ok(s);
        }

        // POST: api/socios
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSocioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Devuelve errores de validación

            var socio = new Socio
            {
                NombreCompleto = dto.NombreCompleto,
                CI = dto.CI,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                ProfesionId = dto.ProfesionId
            };

            _db.Socios.Add(socio);
            await _db.SaveChangesAsync();

            var result = new SocioDto
            {
                Id = socio.Id,
                NombreCompleto = socio.NombreCompleto,
                CI = socio.CI,
                Direccion = socio.Direccion,
                Telefono = socio.Telefono,
                ProfesionNombre = (await _db.Profesiones.FindAsync(socio.ProfesionId))!.Nombre
            };

            return CreatedAtAction(nameof(GetById), new { id = socio.Id }, result);
        }


        // DELETE: api/socios/1
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var socio = await _db.Socios.FindAsync(id);
            if (socio == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Socio no encontrado"
                });
            }

            _db.Socios.Remove(socio);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                success = true,
                message = "Socio eliminado correctamente"
            });
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSocioDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState); // Devuelve errores de validación

            var socio = await _db.Socios.FindAsync(id);
            if (socio == null) return NotFound(new { message = $"Socio con id {id} no encontrado" });

            socio.NombreCompleto = dto.NombreCompleto;
            socio.CI = dto.CI;
            socio.Direccion = dto.Direccion;
            socio.Telefono = dto.Telefono;
            socio.ProfesionId = dto.ProfesionId;

            await _db.SaveChangesAsync();

            var result = new SocioDto
            {
                Id = socio.Id,
                NombreCompleto = socio.NombreCompleto,
                CI = socio.CI,
                Direccion = socio.Direccion,
                Telefono = socio.Telefono,
                ProfesionNombre = (await _db.Profesiones.FindAsync(socio.ProfesionId))!.Nombre
            };

            return Ok(result);
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("register")]
        public async Task<IActionResult> CreateSocio([FromBody] CreateSocioWithUserDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    status = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                });

            // Validar email
            if (await _db.Users.AnyAsync(u => u.Email == dto.Email))
                return BadRequest(new { status = false, message = "Email ya registrado" });

            // Crear usuario vinculado
            var user = new User
            {
                Nombre = dto.NombreCompleto,
                Email = dto.Email,
                Rol = "Socio"
            };
            user.PasswordHash = _hasher.HashPassword(user, dto.Password);
            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            // Crear socio vinculado al usuario
            var socio = new Socio
            {
                UserId = user.Id,
                NombreCompleto = dto.NombreCompleto,
                CI = dto.CI,
                Direccion = dto.Direccion,
                Telefono = dto.Telefono,
                ProfesionId = dto.ProfesionId
            };
            _db.Socios.Add(socio);
            await _db.SaveChangesAsync();

            return Ok(new
            {
                status = true,
                data = new
                {
                    UserId = user.Id,
                    SocioId = socio.Id,
                    NombreCompleto = socio.NombreCompleto,
                    Email = user.Email
                }
            });
        }



    }
}
