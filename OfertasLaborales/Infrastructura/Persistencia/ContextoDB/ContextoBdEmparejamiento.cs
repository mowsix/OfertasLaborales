// Gestiona la sesión con la BD propia (almacena Emparejamiento, NotificacionEmparejamiento).
using Microsoft.EntityFrameworkCore;
using Dominio.Agregados; // Namespace de Emparejamiento

namespace Infraestructura.Persistencia;

public class ContextoBdEmparejamiento : DbContext
{
    // --- Añade esta línea ---
    public DbSet<Emparejamiento> Emparejamientos { get; set; }
    // -----------------------

    public ContextoBdEmparejamiento(DbContextOptions<ContextoBdEmparejamiento> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Busca y aplica todas las configuraciones de IEntityTypeConfiguration<TEntity>
        // en el ensamblado actual (Infraestructura)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoBdEmparejamiento).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}