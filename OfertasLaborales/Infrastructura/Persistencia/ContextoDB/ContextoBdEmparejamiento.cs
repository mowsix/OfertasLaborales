using Dominio.Agregados;
using Microsoft.EntityFrameworkCore;

namespace Infraestructura.Persistencia.ContextoDB;

public class ContextoBdEmparejamiento : DbContext
{
    public DbSet<Emparejamiento> Emparejamientos { get; set; }

    public ContextoBdEmparejamiento(DbContextOptions<ContextoBdEmparejamiento> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configuración de la entidad Emparejamiento
        modelBuilder.Entity<Emparejamiento>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.IdVacante).IsRequired();
            entity.Property(e => e.EmailCandidato).IsRequired().HasMaxLength(255);
            entity.Property(e => e.Puntuacion).IsRequired();
            entity.Property(e => e.FechaCalculo).IsRequired();
            entity.Property(e => e.Estado).IsRequired().HasMaxLength(20);

            // Crear un índice compuesto para búsquedas eficientes
            entity.HasIndex(e => new { e.IdVacante, e.EmailCandidato }).IsUnique();
        });

        base.OnModelCreating(modelBuilder);
    }
}