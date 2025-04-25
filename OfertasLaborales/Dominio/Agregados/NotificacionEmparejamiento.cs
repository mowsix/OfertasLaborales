// Gestiona el estado (Pendiente, Enviada, Fallida) de un intento de notificación sobre un Emparejamiento.
using Dominio.ObjetosValor;

namespace Dominio.Agregados;

public class NotificacionEmparejamiento
{
    public Guid Id { get; private set; }
    public Guid IdEmparejamiento { get; private set; } // Vincula al Emparejamiento original
    public Guid IdVacanteExterna { get; private set; }
    public string IdCandidatoExterno { get; private set; } // Email
    public EstadoNotificacion Estado { get; private set; }
    public DateTime FechaCreacion { get; private set; }
    public DateTime? FechaUltimoIntento { get; private set; }
    public string? MensajeError { get; private set; }

    protected NotificacionEmparejamiento() {
        IdCandidatoExterno = string.Empty; // EF Core init
    }

    private NotificacionEmparejamiento(Guid idEmparejamiento, Guid idVacanteExterna, string idCandidatoExterno)
    {
        if (idEmparejamiento == Guid.Empty) throw new ArgumentException("ID Emparejamiento inválido");
        if (idVacanteExterna == Guid.Empty) throw new ArgumentException("ID Vacante inválido");
        if (string.IsNullOrWhiteSpace(idCandidatoExterno)) throw new ArgumentException("ID Candidato (Email) inválido");

        Id = Guid.NewGuid();
        IdEmparejamiento = idEmparejamiento;
        IdVacanteExterna = idVacanteExterna;
        IdCandidatoExterno = idCandidatoExterno;
        Estado = EstadoNotificacion.Pendiente; // Estado inicial
        FechaCreacion = DateTime.UtcNow;
        FechaUltimoIntento = null;
        MensajeError = null;
    }

    public static NotificacionEmparejamiento CrearNueva(Emparejamiento emparejamiento)
    {
         if (emparejamiento == null) throw new ArgumentNullException(nameof(emparejamiento));
         return new NotificacionEmparejamiento(
             emparejamiento.Id,
             emparejamiento.IdVacanteExterna,
             emparejamiento.IdCandidatoExterno
         );
    }

    public void MarcarComoEnviada()
    {
        if (Estado != EstadoNotificacion.Fallida) // Permitimos reintentar fallidas, pero no reenviar exitosas? O sí? Decisión de negocio.
        {
             Estado = EstadoNotificacion.Enviada;
             FechaUltimoIntento = DateTime.UtcNow;
             MensajeError = null; // Limpiar error si reintento fue exitoso
        }
    }

    public void MarcarComoFallida(string? mensajeError)
    {
        Estado = EstadoNotificacion.Fallida;
        FechaUltimoIntento = DateTime.UtcNow;
        MensajeError = mensajeError?.Length > 500 ? mensajeError.Substring(0, 500) : mensajeError; // Truncar error largo
    }
}