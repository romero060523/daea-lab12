using ApiMatriculas.Data;
using ApiMatriculas.DTOs;
using ApiMatriculas.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiMatriculas.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InstructoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public InstructoresController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Lista los instructores. Por defecto solo los activos.</summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Instructor>>> GetInstructores(
        [FromQuery] bool incluirInactivos = false)
    {
        var query = _context.Instructores.AsNoTracking();

        if (!incluirInactivos)
            query = query.Where(i => i.Activo);

        var lista = await query
            .OrderBy(i => i.Apellidos)
            .ThenBy(i => i.Nombres)
            .ToListAsync();

        return Ok(lista);
    }

    /// <summary>Obtiene un instructor por su id.</summary>
    [HttpGet("{id:int}")]
    public async Task<ActionResult<Instructor>> GetInstructor(int id)
    {
        var instructor = await _context.Instructores
            .AsNoTracking()
            .FirstOrDefaultAsync(i => i.IdInstructor == id);

        if (instructor is null)
            return NotFound(new { mensaje = $"No existe un instructor con id {id}." });

        return Ok(instructor);
    }

    /// <summary>Registra un nuevo instructor.</summary>
    [HttpPost]
    public async Task<ActionResult<Instructor>> CrearInstructor(InstructorDto dto)
    {
        var instructor = new Instructor
        {
            Nombres = dto.Nombres,
            Apellidos = dto.Apellidos,
            Especialidad = dto.Especialidad,
            Email = dto.Email,
            Activo = true
        };

        _context.Instructores.Add(instructor);
        await _context.SaveChangesAsync();

        return CreatedAtAction(
            nameof(GetInstructor),
            new { id = instructor.IdInstructor },
            instructor);
    }

    /// <summary>Actualiza los datos de un instructor existente.</summary>
    [HttpPut("{id:int}")]
    public async Task<IActionResult> ActualizarInstructor(int id, InstructorDto dto)
    {
        var instructor = await _context.Instructores.FindAsync(id);

        if (instructor is null)
            return NotFound(new { mensaje = $"No existe un instructor con id {id}." });

        instructor.Nombres = dto.Nombres;
        instructor.Apellidos = dto.Apellidos;
        instructor.Especialidad = dto.Especialidad;
        instructor.Email = dto.Email;

        await _context.SaveChangesAsync();
        return NoContent();
    }

    /// <summary>Desactiva un instructor (borrado lógico).</summary>
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DesactivarInstructor(int id)
    {
        var instructor = await _context.Instructores.FindAsync(id);

        if (instructor is null)
            return NotFound(new { mensaje = $"No existe un instructor con id {id}." });

        if (!instructor.Activo)
            return BadRequest(new { mensaje = "El instructor ya se encuentra inactivo." });

        instructor.Activo = false;
        await _context.SaveChangesAsync();

        return NoContent();
    }

    /// <summary>Reactiva un instructor previamente desactivado.</summary>
    [HttpPatch("{id:int}/activar")]
    public async Task<IActionResult> ActivarInstructor(int id)
    {
        var instructor = await _context.Instructores.FindAsync(id);

        if (instructor is null)
            return NotFound(new { mensaje = $"No existe un instructor con id {id}." });

        instructor.Activo = true;
        await _context.SaveChangesAsync();

        return NoContent();
    }
}