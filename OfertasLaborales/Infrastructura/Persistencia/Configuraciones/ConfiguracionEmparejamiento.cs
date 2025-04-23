// Mapeos Fluent API para las entidades propias en EF Core.
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Dominio.Agregados; // Namespace de Emparejamiento

namespace Infraestructura.Persistencia.Configuraciones;

public class ConfiguracionEmparejamiento : IEntityTypeConfiguration<Emparejamiento>
{
    public void Configure(EntityTypeBuilder<Emparejamiento> builder)
    {
        // Nombre de la tabla
        builder.ToTable("Emparejamientos");

        // Clave Primaria
        builder.HasKey(e => e.Id);

        // Propiedades del Agregado
        builder.Property(e => e.IdVacanteExterna).IsRequired();
        builder.Property(e => e.IdCandidatoExterno).IsRequired().HasMaxLength(255); // Email como ID
        builder.Property(e => e.FechaCalculo).IsRequired();

        // Mapeo del Objeto de Valor PuntuacionEmparejamiento como "Owned Entity"
        // Esto hará que sus propiedades se mapeen como columnas en la misma tabla Emparejamientos
        builder.OwnsOne(e => e.Puntuacion, puntuacionBuilder =>
        {
            // Mapear las propiedades de PuntuacionEmparejamiento a columnas
            puntuacionBuilder.Property(p => p.PuntuacionTotal)
                             .HasColumnName("PuntuacionTotal") // Nombre explícito de columna
                             .IsRequired();

            // Si tuvieras DesglosePuntos (Dictionary), su mapeo sería más complejo
            // (ej. convertir a JSON string o tabla separada) - Omitido por simplicidad
        });

        // Índices (opcional pero recomendado para consultas)
        builder.HasIndex(e => e.IdVacanteExterna);
        builder.HasIndex(e => e.IdCandidatoExterno);
        builder.HasIndex(e => e.FechaCalculo);
    }
}