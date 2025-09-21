using Microsoft.EntityFrameworkCore;
using CooperativaApi.Data;
using CooperativaApi.DTOs;
using CooperativaApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CooperativaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AportacionesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public AportacionesController(AppDbContext db) => _db = db;

        // GET: api/aportaciones -> Admin ve todas
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Aportaciones
                .Include(a => a.Socio)
                .Select(a => new AportacionDto
                {
                    Id = a.Id,
                    Monto = a.Monto,
                    Fecha = a.Fecha,
                    SocioNombre = a.Socio!.NombreCompleto
                })
                .ToListAsync();

            return Ok(list);
        }

        // POST: api/aportaciones -> Socio crea su aportación
        [Authorize(Roles = "Socio")]
        [HttpPost]
        public async Task<IActionResult> Crear(CreateAportacionDto dto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Devolver errores si hay datos inválidos
            }


            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var socio = await _db.Socios.FirstOrDefaultAsync(s => s.UserId == userId);
            if (socio == null) return Unauthorized(new { message = "No autorizado para este socio." });

            var aportacion = new Aportacion
            {
                SocioId = socio.Id,
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow
            };

            _db.Aportaciones.Add(aportacion);
            await _db.SaveChangesAsync();

            var result = new AportacionDto
            {
                Id = aportacion.Id,
                Monto = aportacion.Monto,
                Fecha = aportacion.Fecha,
                SocioNombre = socio.NombreCompleto
            };

            return CreatedAtAction(nameof(GetById), new { id = aportacion.Id }, result);
        }

        // GET: api/aportaciones/{id}
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var aportacion = await _db.Aportaciones
                .Include(a => a.Socio)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (aportacion == null) return NotFound();

            var result = new AportacionDto
            {
                Id = aportacion.Id,
                Monto = aportacion.Monto,
                Fecha = aportacion.Fecha,
                SocioNombre = aportacion.Socio?.NombreCompleto ?? ""
            };

            return Ok(result);
        }

        // PUT: api/aportaciones/{id} -> Solo Admin puede editar
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(int id, CreateAportacionDto dto)
        {
            var aportacion = await _db.Aportaciones.FindAsync(id);
            if (aportacion == null) return NotFound();

            aportacion.Monto = dto.Monto;
            await _db.SaveChangesAsync();

            return Ok(new { message = "Aportación actualizada" });
        }

        // DELETE: api/aportaciones/{id} -> Solo Admin puede eliminar
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var aportacion = await _db.Aportaciones.FindAsync(id);
            if (aportacion == null) return NotFound();

            _db.Aportaciones.Remove(aportacion);
            await _db.SaveChangesAsync();

            return Ok(new { message = "Aportación eliminada" });
        }


        [Authorize(Roles = "Admin")]
        [HttpPost("admin")]
        public async Task<IActionResult> CrearPorAdmin(CreateAportacionByAdminDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var socio = await _db.Socios.FindAsync(dto.SocioId);
            if (socio == null)
                return NotFound(new { message = "Socio no encontrado" });

            var aportacion = new Aportacion
            {
                SocioId = socio.Id,
                Monto = dto.Monto,
                Fecha = DateTime.UtcNow
            };

            _db.Aportaciones.Add(aportacion);
            await _db.SaveChangesAsync();

            var result = new AportacionDto
            {
                Id = aportacion.Id,
                Monto = aportacion.Monto,
                Fecha = aportacion.Fecha,
                SocioNombre = socio.NombreCompleto
            };

            return CreatedAtAction(nameof(GetById), new { id = aportacion.Id }, result);
        }


    }
}//             await _db.SaveChangesAsync();s
