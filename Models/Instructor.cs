using System.ComponentModel.DataAnnotations;

namespace ApiMatriculas.Models;

public class Instructor
{
    [Key]
    public int IdInstructor { get; set; }

    [Required, MaxLength(100)]
    public string Nombres { get; set; } = string.Empty;

    [Required, MaxLength(100)]
    public string Apellidos { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? Especialidad { get; set; }

    [MaxLength(150), EmailAddress]
    public string? Email { get; set; }

    public bool Activo { get; set; } = true;

    public ICollection<Curso> Cursos { get; set; } = new List<Curso>();
}