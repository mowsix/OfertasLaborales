// Contrato para obtener datos de Candidatos externos.
using Dominio.ModelosLectura;

namespace Dominio.Interfaces;

public interface IFuenteDatosExternaCandidatos
{
    Task<IEnumerable<CandidatoExterno>> ObtenerCandidatosAsync(CancellationToken cancellationToken = default);
}