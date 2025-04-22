using Dominio.Agregados;

namespace Dominio.Interfaces;

public interface IRepositorioEmparejamientos
{
    Task<Emparejamiento?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Emparejamiento>> ObtenerPorVacanteAsync(Guid idVacante, CancellationToken cancellationToken = default);
    Task<IEnumerable<Emparejamiento>> ObtenerPorCandidatoAsync(string emailCandidato, CancellationToken cancellationToken = default);
    Task<IEnumerable<Emparejamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default);
    Task AgregarAsync(Emparejamiento emparejamiento, CancellationToken cancellationToken = default);
    void Actualizar(Emparejamiento emparejamiento);
    void Eliminar(Emparejamiento emparejamiento);
}