using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiMatriculas.Models;

public class Pago
{
    [Key]
    public int IdPago { get; set; }

    public int IdMatricula { get; set; }

    [ForeignKey(nameof(IdMatricula))]
    public Matricula? Matricula { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Monto { get; set; }

    public DateTime FechaPago { get; set; } = DateTime.UtcNow;

    [MaxLength(50)]
    public string MetodoPago { get; set; } = string.Empty;

    [MaxLength(30)]
    public string EstadoPago { get; set; } = "Pendiente";
}