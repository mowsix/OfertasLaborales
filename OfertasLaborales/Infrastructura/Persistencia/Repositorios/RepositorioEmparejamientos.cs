// Implementa IRepositorioEmparejamientos (guarda/lee en BD propia).
using Microsoft.EntityFrameworkCore; // Para Include, ToListAsync, etc.
using Dominio.Agregados;
using Dominio.Interfaces;
using Infraestructura.Persistencia; // Para el Contexto

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioEmparejamientos : IRepositorioEmparejamientos
{
    private readonly ContextoBdEmparejamiento _contexto;

    public RepositorioEmparejamientos(ContextoBdEmparejamiento contexto)
    {
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
    }

    public async Task AgregarAsync(Emparejamiento emparejamiento, CancellationToken cancellationToken = default)
    {
        await _contexto.Emparejamientos.AddAsync(emparejamiento, cancellationToken);
    }

    public async Task<Emparejamiento?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        // Include(e => e.Puntuacion) no es necesario si Puntuacion es Owned Entity (EF lo incluye por defecto)
        return await _contexto.Emparejamientos.FindAsync(new object[] { id }, cancellationToken);
    }

     public async Task<IEnumerable<Emparejamiento>> ObtenerPorIdVacanteAsync(Guid idVacanteExterna, CancellationToken cancellationToken = default)
    {
        return await _contexto.Emparejamientos
                              .Where(e => e.IdVacanteExterna == idVacanteExterna)
                              .OrderByDescending(e => e.Puntuacion.PuntuacionTotal) // Ordenar por puntaje
                              .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
         return await _contexto.Emparejamientos
                              .OrderByDescending(e => e.FechaCalculo) // Ordenar por fecha reciente
                              .ToListAsync(cancellationToken);
    }

    public async Task LimpiarTodosAsync(CancellationToken cancellationToken = default)
    {
        // Cuidado: Esto borra toda la tabla. Usar con precaución.
         var todos = await _contexto.Emparejamientos.ToListAsync(cancellationToken);
         if(todos.Any())
         {
             _contexto.Emparejamientos.RemoveRange(todos);
             // Nota: SaveChangesAsync se llama desde la Unidad de Trabajo
         }
    }
}