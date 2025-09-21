using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CooperativaApi.Data;
using CooperativaApi.Models;
using CooperativaApi.DTOs;
using System.Security.Claims;

namespace CooperativaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PrestamosController : ControllerBase
    {
        private readonly AppDbContext _db;
        public PrestamosController(AppDbContext db) => _db = db;

        // GET: api/prestamos
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Prestamos
                .Include(p => p.Socio)
                .Select(p => new PrestamoDto
                {
                    Id = p.Id,
                    Monto = p.Monto,
                    PlazoMeses = p.PlazoMeses,
                    Estado = p.Estado,
                    SocioNombre = p.Socio.NombreCompleto
                })
                .ToListAsync();

            return Ok(new { status = true, data = list });
        }

        // POST: api/prestamos/solicitar
        [Authorize(Roles = "Socio")]
        [HttpPost("solicitar")]
        public async Task<IActionResult> Solicitar([FromBody] CreatePrestamoDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    status = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var socio = await _db.Socios.FirstOrDefaultAsync(s => s.UserId == userId);
            if (socio == null)
                return Unauthorized(new { status = false, message = "No autorizado para este socio." });

            var prestamo = new Prestamo
            {
                Monto = dto.Monto,
                PlazoMeses = dto.PlazoMeses,
                Estado = "Pendiente",
                SocioId = socio.Id
            };

            _db.Prestamos.Add(prestamo);
            await _db.SaveChangesAsync();

            var result = new PrestamoDto
            {
                Id = prestamo.Id,
                Monto = prestamo.Monto,
                PlazoMeses = prestamo.PlazoMeses,
                Estado = prestamo.Estado,
                SocioNombre = socio.NombreCompleto
            };

            return CreatedAtAction(nameof(GetById), new { id = prestamo.Id }, new { status = true, data = result });
        }

        // PUT: api/prestamos/{id}/aprobar
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}/aprobar")]
        public async Task<IActionResult> Aprobar(int id)
        {
            var prestamo = await _db.Prestamos.FindAsync(id);
            if (prestamo == null)
                return NotFound(new { status = false, message = "Préstamo no encontrado" });

            prestamo.Estado = "Aprobado";
            await _db.SaveChangesAsync();

            var socio = await _db.Socios.FindAsync(prestamo.SocioId);

            var result = new PrestamoDto
            {
                Id = prestamo.Id,
                Monto = prestamo.Monto,
                PlazoMeses = prestamo.PlazoMeses,
                Estado = prestamo.Estado,
                SocioNombre = socio?.NombreCompleto ?? ""
            };

            return Ok(new { status = true, data = result });
        }

        // GET: api/prestamos/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var prestamo = await _db.Prestamos
                .Include(p => p.Socio)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (prestamo == null)
                return NotFound(new { status = false, message = "Préstamo no encontrado" });

            var result = new PrestamoDto
            {
                Id = prestamo.Id,
                Monto = prestamo.Monto,
                PlazoMeses = prestamo.PlazoMeses,
                Estado = prestamo.Estado,
                SocioNombre = prestamo.Socio?.NombreCompleto ?? ""
            };

            return Ok(new { status = true, data = result });
        }
    }
}
