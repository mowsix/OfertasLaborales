// Contrato para obtener datos de Vacantes externas.
using Dominio.ModelosLectura;

namespace Dominio.Interfaces;

public interface IFuenteDatosExternaVacantes
{
    Task<IEnumerable<VacanteExterna>> ObtenerVacantesAsync(CancellationToken cancellationToken = default);
}