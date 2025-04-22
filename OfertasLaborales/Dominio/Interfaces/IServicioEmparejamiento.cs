using Dominio.ModelosLectura;

namespace Dominio.Interfaces;

public interface IServicioEmparejamiento
{
    Task<Dictionary<Guid, Dictionary<string, double>>> CalcularEmparejamientos(
        IEnumerable<VacanteExterna> vacantes,
        IEnumerable<CandidatoExterno> candidatos,
        CancellationToken cancellationToken = default);
}