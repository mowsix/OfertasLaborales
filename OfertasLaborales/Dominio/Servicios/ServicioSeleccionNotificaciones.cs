// Implementa las reglas para elegir qué emparejamientos notificar.
using Dominio.Agregados;
using Dominio.Interfaces;

namespace Dominio.Servicios;

public class ServicioSeleccionNotificaciones : IServicioSeleccionNotificaciones
{
    private readonly IRepositorioEmparejamientos _repoEmparejamientos;
    private readonly IRepositorioNotificacionesEmparejamiento _repoNotificaciones;

    public ServicioSeleccionNotificaciones(
        IRepositorioEmparejamientos repoEmparejamientos,
        IRepositorioNotificacionesEmparejamiento repoNotificaciones)
    {
        _repoEmparejamientos = repoEmparejamientos;
        _repoNotificaciones = repoNotificaciones;
    }

    public async Task<IEnumerable<Emparejamiento>> SeleccionarEmparejamientosParaNotificarAsync(CancellationToken cancellationToken = default)
    {
        // 1. Obtener todos los emparejamientos calculados
        var todosEmparejamientos = await _repoEmparejamientos.ObtenerTodosAsync(cancellationToken);

        // 2. Obtener los IDs de los emparejamientos que ya fueron notificados con éxito
        var notificacionesEnviadas = await _repoNotificaciones.ObtenerEnviadasAsync(cancellationToken);
        var idsEmparejamientosYaNotificados = notificacionesEnviadas.Select(n => n.IdEmparejamiento).ToHashSet();

        // 3. Filtrar: Devolver solo los emparejamientos cuyo ID NO esté en la lista de notificados
        var emparejamientosPendientes = todosEmparejamientos
            .Where(e => !idsEmparejamientosYaNotificados.Contains(e.Id));

        return emparejamientosPendientes;
    }
}