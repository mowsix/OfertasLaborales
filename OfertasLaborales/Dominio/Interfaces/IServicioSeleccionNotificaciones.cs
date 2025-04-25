// Contrato para la lógica que decide qué emparejamientos notificar.
using Dominio.Agregados;

namespace Dominio.Interfaces;

public interface IServicioSeleccionNotificaciones
{
    /// <summary>
    /// Obtiene los Emparejamientos que están pendientes de notificar.
    /// </summary>
    Task<IEnumerable<Emparejamiento>> SeleccionarEmparejamientosParaNotificarAsync(CancellationToken cancellationToken = default);
}