using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Consultas; // Namespace de tu consulta

namespace API.Controladores;

[ApiController]
[Route("api/[controller]")]
public class DatosPruebaController : ControllerBase
{
    private readonly IMediator _mediator;

    public DatosPruebaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("contar-externos")] // Ruta: /api/DatosPrueba/contar-externos
    [ProducesResponseType(typeof(Aplicacion.DTOs.ResumenDatosExternosDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ContarDatosExternos()
    {
        try
        {
            var consulta = new ConsultaContarDatosExternos();
            var resultado = await _mediator.Send(consulta);
            return Ok(resultado);
        }
        catch (Exception ex)
        {
            // En un caso real, loguearías el error
            return StatusCode(StatusCodes.Status500InternalServerError, $"Error interno: {ex.Message}");
        }
    }
}