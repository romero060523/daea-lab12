using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMatriculas.Models;

public class Matricula
{
    [Key]
    public int IdMatricula { get; set; }

    public int IdEstudiante { get; set; }

    [ForeignKey(nameof(IdEstudiante))]
    public Estudiante? Estudiante { get; set; }

    public int IdCurso { get; set; }

    [ForeignKey(nameof(IdCurso))]
    public Curso? Curso { get; set; }

    public DateTime FechaMatricula { get; set; } = DateTime.UtcNow;

    [MaxLength(30)]
    public string Estado { get; set; } = "Pendiente";

    [Column(TypeName = "decimal(10,2)")]
    public decimal MontoTotal { get; set; }

    public ICollection<Pago> Pagos { get; set; } = new List<Pago>();
}