using System.ComponentModel.DataAnnotations;

namespace ApiMatriculas.DTOs;

public class InstructorDto
{
    [Required(ErrorMessage = "Los nombres son obligatorios")]
    [MaxLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required(ErrorMessage = "Los apellidos son obligatorios")]
    [MaxLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Especialidad { get; set; }

    [EmailAddress(ErrorMessage = "El formato del email no es válido")]
    [MaxLength(150)]
    public string? Email { get; set; }
}