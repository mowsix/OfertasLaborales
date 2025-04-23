// Endpoint para consultar resultados (ej. GET /emparejamientos?candidatoId=X).
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Aplicacion.Comandos; // Para ComandoCalcular...
using Aplicacion.Consultas; // Para ConsultaObtener...
using Aplicacion.DTOs; // Para los DTOs de respuesta

namespace API.Controladores; // Asegúrate que el namespace sea correcto

[ApiController]
// Se define la ruta explícitamente para mantener el estándar RESTful en la URL,
// aunque el nombre del controlador esté en español.
[Route("api/emparejamientos")]
public class ControladorEmparejamientos : ControllerBase // Nombre de clase cambiado
{
    private readonly IMediator _mediator;

    // Constructor no cambia
    public ControladorEmparejamientos(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Calcula y guarda los emparejamientos entre todas las vacantes y candidatos.
    /// Elimina los emparejamientos anteriores antes de calcular.
    /// </summary>
    /// <param name="umbral">Puntuación mínima requerida para considerar un emparejamiento válido.</param>
    /// <returns>El número de emparejamientos creados.</returns>
    [HttpPost("calcular")] // Ruta: POST /api/emparejamientos/calcular?umbral=50
    [ProducesResponseType(typeof(ResultadoCalculoDTO), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CalcularEmparejamientos([FromQuery] int umbral = 50) // Nombre método no cambia
    {
        if (umbral < 0) return BadRequest("El umbral no puede ser negativo.");

        var comando = new ComandoCalcularYGuardarEmparejamientos(umbral);
        var resultado = await _mediator.Send(comando);

        if(resultado.EmparejamientosCreados < 0)
            return StatusCode(StatusCodes.Status500InternalServerError, "Ocurrió un error durante el cálculo.");

        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene todos los emparejamientos calculados y guardados.
    /// </summary>
    /// <returns>Una lista de emparejamientos.</returns>
    [HttpGet] // Ruta: GET /api/emparejamientos
    [ProducesResponseType(typeof(IEnumerable<EmparejamientoDetalleDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerTodos() // Nombre método no cambia
    {
        var consulta = new ConsultaObtenerTodosEmparejamientos();
        var resultado = await _mediator.Send(consulta);
        return Ok(resultado);
    }

    /// <summary>
    /// Obtiene los candidatos emparejados para una vacante específica, ordenados por puntuación.
    /// </summary>
    /// <param name="idVacante">El ID externo de la vacante.</param>
    /// <returns>Una lista de emparejamientos para esa vacante.</returns>
    // Se ajusta la ruta para que siga usando el ID de la vacante
    [HttpGet("por-vacante/{idVacante:guid}")] // Ruta: GET /api/emparejamientos/por-vacante/guid-de-la-vacante
    [ProducesResponseType(typeof(IEnumerable<EmparejamientoDetalleDTO>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObtenerPorVacante(Guid idVacante) // Nombre método no cambia
    {
         if (idVacante == Guid.Empty) return BadRequest("El ID de vacante no es válido.");

        var consulta = new ConsultaObtenerEmparejamientosPorVacante(idVacante);
        var resultado = await _mediator.Send(consulta);

        // if (!resultado.Any()) return NotFound("No se encontraron emparejamientos para esta vacante."); // Opcional

        return Ok(resultado);
    }
}
