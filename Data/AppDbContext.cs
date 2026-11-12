using ApiMatriculas.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiMatriculas.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Instructor> Instructores => Set<Instructor>();
    public DbSet<Estudiante> Estudiantes => Set<Estudiante>();
    public DbSet<Curso> Cursos => Set<Curso>();
    public DbSet<Matricula> Matriculas => Set<Matricula>();
    public DbSet<Pago> Pagos => Set<Pago>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Instructor>().ToTable("Instructor");
        modelBuilder.Entity<Estudiante>().ToTable("Estudiante");
        modelBuilder.Entity<Curso>().ToTable("Curso");
        modelBuilder.Entity<Matricula>().ToTable("Matricula");
        modelBuilder.Entity<Pago>().ToTable("Pago");

        // Email único por estudiante
        modelBuilder.Entity<Estudiante>()
            .HasIndex(e => e.Email)
            .IsUnique();

        // No permitir borrar un instructor que tiene cursos
        modelBuilder.Entity<Curso>()
            .HasOne(c => c.Instructor)
            .WithMany(i => i.Cursos)
            .HasForeignKey(c => c.IdInstructor)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Matricula>()
            .HasOne(m => m.Estudiante)
            .WithMany(e => e.Matriculas)
            .HasForeignKey(m => m.IdEstudiante)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Matricula>()
            .HasOne(m => m.Curso)
            .WithMany(c => c.Matriculas)
            .HasForeignKey(m => m.IdCurso)
            .OnDelete(DeleteBehavior.Restrict);

        // Si se borra la matrícula, se borran sus pagos
        modelBuilder.Entity<Pago>()
            .HasOne(p => p.Matricula)
            .WithMany(m => m.Pagos)
            .HasForeignKey(p => p.IdMatricula)
            .OnDelete(DeleteBehavior.Cascade);
    }
}