 // Mapeos Fluent API para las entidades propias en EF Core
 using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Agregados;
using Dominio.ObjetosValor; // Para EstadoNotificacion

namespace Infraestructura.Persistencia.Configuraciones;

public class ConfiguracionNotificacionEmparejamiento : IEntityTypeConfiguration<NotificacionEmparejamiento>
{
    public void Configure(EntityTypeBuilder<NotificacionEmparejamiento> builder)
    {
        builder.ToTable("NotificacionesEmparejamiento");

        builder.HasKey(n => n.Id);

        builder.Property(n => n.IdEmparejamiento).IsRequired();
        builder.Property(n => n.IdVacanteExterna).IsRequired();
        builder.Property(n => n.IdCandidatoExterno).IsRequired().HasMaxLength(255); // Email
        builder.Property(n => n.FechaCreacion).IsRequired();
        builder.Property(n => n.FechaUltimoIntento); // Nullable
        builder.Property(n => n.MensajeError).HasMaxLength(500); // Limitar tamaño

        // Mapear el Enum EstadoNotificacion
        // Guardarlo como string en la BD es más legible
        builder.Property(n => n.Estado)
               .IsRequired()
               .HasConversion(
                   v => v.ToString(), // Convertir Enum a String para guardar
                   v => (EstadoNotificacion)Enum.Parse(typeof(EstadoNotificacion), v) // Convertir String de BD a Enum
               )
               .HasMaxLength(50); // Tamaño suficiente para los nombres del enum

        // Índices útiles
        builder.HasIndex(n => n.IdEmparejamiento).IsUnique(); // Solo una notificación por emparejamiento
        builder.HasIndex(n => n.Estado);
    }
}