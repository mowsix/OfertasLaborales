 // Contrato para persistir/consultar Emparejamientos.
 using Dominio.Agregados;

namespace Dominio.Interfaces;

public interface IRepositorioEmparejamientos
{
    Task AgregarAsync(Emparejamiento emparejamiento, CancellationToken cancellationToken = default);
    Task<Emparejamiento?> ObtenerPorIdAsync(Guid id, CancellationToken cancellationToken = default); // Añadido por si acaso
    Task<IEnumerable<Emparejamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Emparejamiento>> ObtenerPorIdVacanteAsync(Guid idVacanteExterna, CancellationToken cancellationToken = default); // Añadido para consulta específica
    Task LimpiarTodosAsync(CancellationToken cancellationToken = default); // Para borrar antes de recalcular
}