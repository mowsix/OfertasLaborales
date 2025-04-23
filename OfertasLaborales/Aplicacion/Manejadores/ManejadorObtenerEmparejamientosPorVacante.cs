using MediatR;
using Aplicacion.Consultas;
using Aplicacion.DTOs;
using Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Manejadores;

public class ManejadorObtenerEmparejamientosPorVacante
    : IRequestHandler<ConsultaObtenerEmparejamientosPorVacante, IEnumerable<EmparejamientoDetalleDTO>>
{
    private readonly IRepositorioEmparejamientos _repositorioEmparejamientos;
    private readonly ILogger<ManejadorObtenerEmparejamientosPorVacante> _logger;
    // Aquí también podrías inyectar IFuenteDatosExternaCandidatos para obtener nombres

    public ManejadorObtenerEmparejamientosPorVacante(
        IRepositorioEmparejamientos repositorioEmparejamientos,
        ILogger<ManejadorObtenerEmparejamientosPorVacante> logger)
    {
        _repositorioEmparejamientos = repositorioEmparejamientos;
        _logger = logger;
    }

    public async Task<IEnumerable<EmparejamientoDetalleDTO>> Handle(ConsultaObtenerEmparejamientosPorVacante request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Obteniendo emparejamientos para vacante ID: {VacanteId}", request.IdVacante);
        var emparejamientos = await _repositorioEmparejamientos.ObtenerPorIdVacanteAsync(request.IdVacante, cancellationToken);

        // Mapeo simple (igual que el anterior, podrías enriquecerlo)
        var resultadoDto = emparejamientos.Select(e => new EmparejamientoDetalleDTO
        {
            IdEmparejamiento = e.Id,
            IdVacanteExterna = e.IdVacanteExterna,
            IdCandidatoExterno = e.IdCandidatoExterno, // Email
            PuntuacionTotal = e.Puntuacion.PuntuacionTotal,
            FechaCalculo = e.FechaCalculo
            // Podrías añadir NombreCandidato aquí si obtienes los datos del candidato
        }).ToList();
        _logger.LogInformation("Se encontraron {Count} emparejamientos para la vacante.", resultadoDto.Count);

        return resultadoDto;
    }
}