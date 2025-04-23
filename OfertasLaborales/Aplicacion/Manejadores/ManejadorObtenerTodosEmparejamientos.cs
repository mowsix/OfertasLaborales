using MediatR;
using Aplicacion.Consultas;
using Aplicacion.DTOs;
using Dominio.Interfaces;
using Microsoft.Extensions.Logging;

namespace Aplicacion.Manejadores;

public class ManejadorObtenerTodosEmparejamientos
    : IRequestHandler<ConsultaObtenerTodosEmparejamientos, IEnumerable<EmparejamientoDetalleDTO>>
{
    private readonly IRepositorioEmparejamientos _repositorioEmparejamientos;
    private readonly ILogger<ManejadorObtenerTodosEmparejamientos> _logger;
     // NOTA: Para enriquecer el DTO con nombres/títulos, necesitarías inyectar
     // IFuenteDatosExternaVacantes e IFuenteDatosExternaCandidatos aquí también.

    public ManejadorObtenerTodosEmparejamientos(
        IRepositorioEmparejamientos repositorioEmparejamientos,
        ILogger<ManejadorObtenerTodosEmparejamientos> logger)
    {
        _repositorioEmparejamientos = repositorioEmparejamientos;
        _logger = logger;
    }

    public async Task<IEnumerable<EmparejamientoDetalleDTO>> Handle(ConsultaObtenerTodosEmparejamientos request, CancellationToken cancellationToken)
    {
         _logger.LogInformation("Obteniendo todos los emparejamientos...");
        var emparejamientos = await _repositorioEmparejamientos.ObtenerTodosAsync(cancellationToken);

        // --- Mapeo Simple a DTO ---
        // En un caso real, aquí podrías obtener los datos de vacantes/candidatos
        // para enriquecer el DTO con nombres, títulos, etc. Por ahora, solo IDs.
        var resultadoDto = emparejamientos.Select(e => new EmparejamientoDetalleDTO
        {
            IdEmparejamiento = e.Id,
            IdVacanteExterna = e.IdVacanteExterna,
            IdCandidatoExterno = e.IdCandidatoExterno, // Email
            PuntuacionTotal = e.Puntuacion.PuntuacionTotal,
            FechaCalculo = e.FechaCalculo
        }).ToList();
         _logger.LogInformation("Se encontraron {Count} emparejamientos.", resultadoDto.Count);

        return resultadoDto;
    }
}