using Microsoft.EntityFrameworkCore;
// Asegúrate que este using incluya ambos agregados o añade los necesarios
using Dominio.Agregados;

namespace Infraestructura.Persistencia;

// El nombre de la clase y el constructor no cambian
public class ContextoBdEmparejamiento : DbContext
{
    // DbSet existente para Emparejamiento
    public DbSet<Emparejamiento> Emparejamientos { get; set; }

    // --- AÑADIR ESTA LÍNEA para la nueva entidad ---
    public DbSet<NotificacionEmparejamiento> NotificacionesEmparejamiento { get; set; }
    // ----------------------------------------------

    // El constructor no cambia
    public ContextoBdEmparejamiento(DbContextOptions<ContextoBdEmparejamiento> options)
        : base(options)
    {
    }

    // OnModelCreating no cambia, ya que `ApplyConfigurationsFromAssembly`
    // encontrará y aplicará automáticamente la nueva clase
    // `ConfiguracionNotificacionEmparejamiento` que creamos antes.
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Busca y aplica todas las configuraciones de IEntityTypeConfiguration<TEntity>
        // en el ensamblado actual (Infraestructura)
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ContextoBdEmparejamiento).Assembly);

        base.OnModelCreating(modelBuilder);
    }
}