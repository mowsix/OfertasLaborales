// Implementa IEnviadorNotificaciones escribiendo en consola.
using Aplicacion.Interfaces; // Para IEnviadorNotificaciones
using Microsoft.Extensions.Logging;

namespace Infraestructura.Notificaciones; // O el namespace que uses

public class EnviadorNotificacionesConsola : IEnviadorNotificaciones
{
    private readonly ILogger<EnviadorNotificacionesConsola> _logger;

    public EnviadorNotificacionesConsola(ILogger<EnviadorNotificacionesConsola> logger)
    {
        _logger = logger;
    }

    public Task EnviarAsync(string destinatario, string asunto, string cuerpo, CancellationToken cancellationToken = default)
    {
        // Simular el envío escribiendo en la consola
         _logger.LogWarning("--- INICIO SIMULACION ENVIO NOTIFICACION ---");
         _logger.LogWarning("Para: {Destinatario}", destinatario);
         _logger.LogWarning("Asunto: {Asunto}", asunto);
         _logger.LogWarning("Cuerpo: {Cuerpo}", cuerpo);
         _logger.LogWarning("--- FIN SIMULACION ENVIO NOTIFICACION ---");

        // En una implementación real (ej. email), aquí iría la lógica de conexión SMTP, etc.
        // y podría lanzar excepciones si falla el envío.
        // Como esto es simulación, asumimos que siempre tiene éxito.
        return Task.CompletedTask;
    }
}