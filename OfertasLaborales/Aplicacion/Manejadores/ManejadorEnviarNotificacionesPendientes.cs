using MediatR;
using Aplicacion.Comandos;
using Aplicacion.DTOs;
using Aplicacion.Interfaces; // Para IEnviadorNotificaciones
using Dominio.Interfaces;
using Dominio.Agregados; // Para NotificacionEmparejamiento
using Dominio.ModelosLectura; // Para VacanteExterna
using Microsoft.Extensions.Logging;
using System.Linq; // Para GroupBy, ToDictionary

namespace Aplicacion.Manejadores;

public class ManejadorEnviarNotificacionesPendientes
    : IRequestHandler<ComandoEnviarNotificacionesPendientes, ResultadoNotificacionDTO>
{
    private readonly IServicioSeleccionNotificaciones _servicioSeleccion;
    private readonly IRepositorioNotificacionesEmparejamiento _repoNotificaciones;
    private readonly IFuenteDatosExternaVacantes _fuenteVacantes; // Para obtener detalles
    private readonly IEnviadorNotificaciones _enviadorNotificaciones;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly ILogger<ManejadorEnviarNotificacionesPendientes> _logger;

    public ManejadorEnviarNotificacionesPendientes(
        IServicioSeleccionNotificaciones servicioSeleccion,
        IRepositorioNotificacionesEmparejamiento repoNotificaciones,
        IFuenteDatosExternaVacantes fuenteVacantes,
        IEnviadorNotificaciones enviadorNotificaciones,
        IUnidadDeTrabajo unidadDeTrabajo,
        ILogger<ManejadorEnviarNotificacionesPendientes> logger)
    {
        _servicioSeleccion = servicioSeleccion;
        _repoNotificaciones = repoNotificaciones;
        _fuenteVacantes = fuenteVacantes;
        _enviadorNotificaciones = enviadorNotificaciones;
        _unidadDeTrabajo = unidadDeTrabajo;
        _logger = logger;
    }

    public async Task<ResultadoNotificacionDTO> Handle(ComandoEnviarNotificacionesPendientes request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando proceso de envío de notificaciones pendientes...");

        var emparejamientosPendientes = (await _servicioSeleccion.SeleccionarEmparejamientosParaNotificarAsync(cancellationToken)).ToList();
        int countSeleccionados = emparejamientosPendientes.Count;
        int countExitosas = 0;
        int countFallidas = 0;

        if (countSeleccionados == 0)
        {
            _logger.LogInformation("No hay emparejamientos pendientes de notificar.");
            return new ResultadoNotificacionDTO(0, 0, 0);
        }

         _logger.LogInformation("Se seleccionaron {Count} emparejamientos para notificar.", countSeleccionados);

        // Optimización: Obtener detalles de las vacantes necesarias una sola vez
        var idsVacantesNecesarias = emparejamientosPendientes.Select(e => e.IdVacanteExterna).Distinct().ToList();
        var todasVacantes = await _fuenteVacantes.ObtenerVacantesAsync(cancellationToken); // Asume que esto ya usa caché si se implementó
        var lookupVacantes = todasVacantes.Where(v => idsVacantesNecesarias.Contains(v.IdExterno))
                                         .ToDictionary(v => v.IdExterno);

        foreach (var emparejamiento in emparejamientosPendientes)
        {
            // Buscar o crear el registro de notificación
            var notificacion = await _repoNotificaciones.ObtenerPorIdEmparejamientoAsync(emparejamiento.Id, cancellationToken);
            bool esNueva = false;
            if (notificacion == null)
            {
                notificacion = NotificacionEmparejamiento.CrearNueva(emparejamiento);
                await _repoNotificaciones.AgregarAsync(notificacion, cancellationToken);
                esNueva = true;
                 _logger.LogInformation("Creando registro de notificación para Emparejamiento ID: {EmparejamientoId}", emparejamiento.Id);
            }
            else if (notificacion.Estado == Dominio.ObjetosValor.EstadoNotificacion.Enviada)
            {
                // Doble chequeo por si acaso la selección tuvo alguna condición de carrera mínima
                _logger.LogWarning("Se intentó notificar un emparejamiento que ya estaba marcado como Enviado. ID: {EmparejamientoId}", emparejamiento.Id);
                continue; // Saltar este
            }

            // Obtener detalles de la vacante
            lookupVacantes.TryGetValue(emparejamiento.IdVacanteExterna, out var vacanteInfo);
            string tituloVacante = vacanteInfo?.Titulo ?? "Vacante Desconocida";
            string empresaVacante = vacanteInfo?.EmpresaContratante ?? "Empresa Desconocida";

            // Construir el mensaje (simplificado)
            string destinatario = emparejamiento.IdCandidatoExterno; // Email
            string asunto = $"¡Hemos encontrado una vacante para ti en {empresaVacante}!";
            string cuerpo = $"Hola,\n\nCreemos que la vacante '{tituloVacante}' en {empresaVacante} podría ser de tu interés basado en tu perfil.\n\nPuedes ver más detalles [aquí](URL_VACANTE_SI_EXISTIERA).\n\nEquipo de Ofertas Laborales."; // Idealmente tendríamos un link

            try
            {
                _logger.LogInformation("Intentando enviar notificación a {Destinatario} sobre Vacante {VacanteId}", destinatario, emparejamiento.IdVacanteExterna);
                await _enviadorNotificaciones.EnviarAsync(destinatario, asunto, cuerpo, cancellationToken);
                notificacion.MarcarComoEnviada();
                countExitosas++;
                 _logger.LogInformation("Notificación enviada exitosamente a {Destinatario}", destinatario);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Fallo al enviar notificación a {Destinatario} para Emparejamiento ID {EmparejamientoId}", destinatario, emparejamiento.Id);
                notificacion.MarcarComoFallida(ex.Message);
                countFallidas++;
            }
             // Si no usamos repo.ActualizarAsync, EF Core tracking debería detectar los cambios en 'notificacion' al llamar a SaveChangesAsync.
        }

        // Guardar todos los cambios en los estados de las notificaciones
        await _unidadDeTrabajo.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Proceso de notificación finalizado. Exitosas: {Exitosas}, Fallidas: {Fallidas}", countExitosas, countFallidas);


        return new ResultadoNotificacionDTO(countSeleccionados, countExitosas, countFallidas);
    }
}