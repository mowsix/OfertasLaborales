 // Endpoint para disparar notificaciones o consultar su estado.
 using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Comandos; // Para ComandoEnviar...
using Aplicacion.DTOs; // Para ResultadoNotificacionDTO

namespace API.Controladores;

[ApiController]
[Route("api/notificaciones")]
public class ControladorNotificaciones : ControllerBase
{
    private readonly IMediator _mediator;

    public ControladorNotificaciones(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Intenta enviar notificaciones por consola a los candidatos
    /// para los emparejamientos que aún no han sido notificados exitosamente.
    /// </summary>
    /// <returns>Un resumen del proceso de envío.</returns>
    [HttpPost("enviar-pendientes")] // POST /api/notificaciones/enviar-pendientes
    [ProducesResponseType(typeof(ResultadoNotificacionDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> EnviarNotificacionesPendientes()
    {
        var comando = new ComandoEnviarNotificacionesPendientes();
        var resultado = await _mediator.Send(comando);
        return Ok(resultado);
    }

    // Aquí podrías añadir un GET para consultar el estado de las notificaciones si quisieras
    // [HttpGet("estado")] ... llamando a una Consulta que use IRepositorioNotificacionesEmparejamiento
}
