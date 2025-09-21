using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using CooperativaApi.Data;
using CooperativaApi.DTOs;

namespace CooperativaApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfesionesController : ControllerBase
    {
        private readonly AppDbContext _db;
        public ProfesionesController(AppDbContext db) => _db = db;

        // GET: api/profesiones -> Admin ve todas
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var list = await _db.Profesiones
                .Select(p => new ProfesionDto { Id = p.Id, Nombre = p.Nombre })
                .ToListAsync();

            return Ok(new
            {
                status = true,
                data = list
            });
        }

        // GET: api/profesiones/{id}
        [Authorize(Roles = "Admin")]
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var profesion = await _db.Profesiones.FindAsync(id);
            if (profesion == null)
                return NotFound(new { status = false, message = "Profesión no encontrada" });

            return Ok(new
            {
                status = true,
                data = new ProfesionDto { Id = profesion.Id, Nombre = profesion.Nombre }
            });
        }

        // POST: api/profesiones -> Crear profesión
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CreateProfesionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    status = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var profesion = new Profesion { Nombre = dto.Nombre };
            _db.Profesiones.Add(profesion);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = profesion.Id }, new
            {
                status = true,
                data = new ProfesionDto { Id = profesion.Id, Nombre = profesion.Nombre }
            });
        }

        // PUT: api/profesiones/{id} -> Editar profesión
        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Editar(int id, [FromBody] CreateProfesionDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(new
                {
                    status = false,
                    errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage)
                });

            var profesion = await _db.Profesiones.FindAsync(id);
            if (profesion == null)
                return NotFound(new { status = false, message = "Profesión no encontrada" });

            profesion.Nombre = dto.Nombre;
            await _db.SaveChangesAsync();

            return Ok(new { status = true, message = "Profesión actualizada" });
        }

        // DELETE: api/profesiones/{id} -> Eliminar profesión
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Eliminar(int id)
        {
            var profesion = await _db.Profesiones.FindAsync(id);
            if (profesion == null)
                return NotFound(new { status = false, message = "Profesión no encontrada" });

            _db.Profesiones.Remove(profesion);
            await _db.SaveChangesAsync();

            return Ok(new { status = true, message = "Profesión eliminada" });
        }
    }
}
