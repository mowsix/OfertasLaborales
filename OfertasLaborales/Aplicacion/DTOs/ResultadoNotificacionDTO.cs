namespace Aplicacion.DTOs;

public record ResultadoNotificacionDTO(
    int EmparejamientosSeleccionados, // Cuántos se intentaron notificar
    int NotificacionesExitosas,
    int NotificacionesFallidas
);
