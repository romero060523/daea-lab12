using ApiMatriculas.Data;
using ApiMatriculas.DTOs;
using ApiMatriculas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMatriculas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstudiantesController : ControllerBase
{
    private readonly AppDbContext _context;

    public EstudiantesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Lista los estudiantes. Por defecto solo los activos.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Estudiante>>> GetEstudiantes(
        [FromQuery] bool incluirInactivos = false)
    {
        var query = _context.Estudiantes.AsNoTracking();

        if (!incluirInactivos)
            query = query.Where(e => e.Activo);

        var lista = await query
            .OrderBy(e => e.Apellidos)
            .ThenBy(e => e.Nombres)
            .ToListAsync();

        return Ok(lista);
    }

    /// <summary>Obtiene un estudiante por su id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Estudiante>> GetEstudiante(int id)
    {
        var estudiante = await _context.Estudiantes
            .AsNoTracking()
            .FirstOrDefaultAsync(e => e.IdEstudiante == id);

        if (estudiante is null)
            return NotFound(new { mensaje = $"No existe un estudiante con id {id}." });

        return Ok(estudiante);
    }

    /// <summary>Registra un nuevo estudiante.</summary>
    [HttpPost]
    public async Task<ActionResult<Estudiante>> CrearEstudiante(EstudianteDto dto)
    {
        var emailEnUso = await _context.Estudiantes
            .AnyAsync(e => e.Email == dto.Email);

        if (emailEnUso)
            return Conflict(new { mensaje = $"El email {dto.Email} ya está registrado." });

        var estudiante = new Estudiante
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Email = dto.Email,
            Telefono = dto.Telefono,
            FechaRegistro = DateTime.UtcNow,
            Activo = true
        };

        _context.Estudiantes.Add(estudiante);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetEstudiante),
            new { id = estudiante.IdEstudiante },
            estudiante);
    }

    /// <summary>Actualiza los datos de un estudiante existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarEstudiante(int id, EstudianteDto dto)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);

        if (estudiante is null)
            return NotFound(new { mensaje = $"No existe un estudiante con id {id}." });

        var emailEnUso = await _context.Estudiantes
            .AnyAsync(e => e.Email == dto.Email && e.IdEstudiante != id);

        if (emailEnUso)
            return Conflict(new { mensaje = $"El email {dto.Email} pertenece a otro estudiante." });

        estudiante.Nombres = dto.Nombres;
        estudiante.Apellidos = dto.Apellidos;
        estudiante.Email = dto.Email;
        estudiante.Telefono = dto.Telefono;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Desactiva un estudiante (borrado lógico).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DesactivarEstudiante(int id)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);

        if (estudiante is null)
            return NotFound(new { mensaje = $"No existe un estudiante con id {id}." });

        if (!estudiante.Activo)
            return BadRequest(new { mensaje = "El estudiante ya se encuentra inactivo." });

        estudiante.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>Reactiva un estudiante previamente desactivado.</summary>
    [HttpPatch("{id:int}/activar")]
    public async Task<IActionResult> ActivarEstudiante(int id)
    {
        var estudiante = await _context.Estudiantes.FindAsync(id);

        if (estudiante is null)
            return NotFound(new { mensaje = $"No existe un estudiante con id {id}." });

        estudiante.Activo = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}