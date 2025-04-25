// Contrato para persistir/consultar Notificaciones.
using Dominio.Agregados;
using Dominio.ObjetosValor; // Para EstadoNotificacion

namespace Dominio.Interfaces;

public interface IRepositorioNotificacionesEmparejamiento
{
    Task AgregarAsync(NotificacionEmparejamiento notificacion, CancellationToken cancellationToken = default);
    Task<NotificacionEmparejamiento?> ObtenerPorIdEmparejamientoAsync(Guid idEmparejamiento, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificacionEmparejamiento>> ObtenerPorEstadoAsync(EstadoNotificacion estado, CancellationToken cancellationToken = default);
    // Podríamos necesitar un método Update si no confiamos solo en el tracking de EF Core/UoW
    // Task ActualizarAsync(NotificacionEmparejamiento notificacion, CancellationToken cancellationToken = default);
    Task<IEnumerable<NotificacionEmparejamiento>> ObtenerEnviadasAsync(CancellationToken cancellationToken = default); // Para el servicio selector
}
