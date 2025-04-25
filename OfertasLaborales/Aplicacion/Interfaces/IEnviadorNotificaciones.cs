// Contrato para el servicio de infraestructura que envía notificaciones (email, SMS, etc.).
namespace Aplicacion.Interfaces; // O el namespace que uses para interfaces de app

public interface IEnviadorNotificaciones
{
    Task EnviarAsync(string destinatario, string asunto, string cuerpo, CancellationToken cancellationToken = default);
}