using Aplicacion.Comandos;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controladores;

[ApiController]
[Route("api/[controller]")]
public class EmparejamientosController : ControllerBase
{
    private readonly IMediator _mediator;

    public EmparejamientosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("calcular")]
    [ProducesResponseType(typeof(ResultadoCalculoEmparejamientos), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalcularEmparejamientos()
    {
        try
        {
            var comando = new ComandoCalcularEmparejamientos();
            var resultado = await _mediator.Send(comando);

            if (resultado.Exito)
                return Ok(resultado);
            else
                return StatusCode(StatusCodes.Status500InternalServerError, resultado);
        }
        catch (Exception ex)
        {
            return StatusCode(StatusCodes.Status500InternalServerError,
                new { Mensaje = $"Error al calcular emparejamientos: {ex.Message}" });
        }
    }

    // Para la siguiente fase: Implementar endpoints para consultar emparejamientos
    [HttpGet("vacante/{idVacante}")]
    public async Task<IActionResult> ObtenerPorVacante(Guid idVacante)
    {
        // TODO: Implementar consulta por vacante
        return Ok(new { Mensaje = "Endpoint por implementar" });
    }

    [HttpGet("candidato/{emailCandidato}")]
    public async Task<IActionResult> ObtenerPorCandidato(string emailCandidato)
    {
        // TODO: Implementar consulta por candidato
        return Ok(new { Mensaje = "Endpoint por implementar" });
    }
}
