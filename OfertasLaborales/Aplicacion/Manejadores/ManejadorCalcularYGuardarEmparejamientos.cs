using MediatR;
using Aplicacion.Comandos;
using Aplicacion.DTOs;
using Dominio.Interfaces;
using Dominio.Agregados; // Para Emparejamiento
using Dominio.ModelosLectura; // Para VacanteExterna, CandidatoExterno
using Microsoft.Extensions.Logging; // Para logging

namespace Aplicacion.Manejadores;

public class ManejadorCalcularYGuardarEmparejamientos
    : IRequestHandler<ComandoCalcularYGuardarEmparejamientos, ResultadoCalculoDTO>
{
    private readonly IFuenteDatosExternaVacantes _fuenteVacantes;
    private readonly IFuenteDatosExternaCandidatos _fuenteCandidatos;
    private readonly IServicioEmparejamiento _servicioEmparejamiento;
    private readonly IRepositorioEmparejamientos _repositorioEmparejamientos;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;
    private readonly ILogger<ManejadorCalcularYGuardarEmparejamientos> _logger;

    public ManejadorCalcularYGuardarEmparejamientos(
        IFuenteDatosExternaVacantes fuenteVacantes,
        IFuenteDatosExternaCandidatos fuenteCandidatos,
        IServicioEmparejamiento servicioEmparejamiento,
        IRepositorioEmparejamientos repositorioEmparejamientos,
        IUnidadDeTrabajo unidadDeTrabajo,
        ILogger<ManejadorCalcularYGuardarEmparejamientos> logger) // Inyectar Logger
    {
        _fuenteVacantes = fuenteVacantes;
        _fuenteCandidatos = fuenteCandidatos;
        _servicioEmparejamiento = servicioEmparejamiento;
        _repositorioEmparejamientos = repositorioEmparejamientos;
        _unidadDeTrabajo = unidadDeTrabajo;
        _logger = logger;
    }

    public async Task<ResultadoCalculoDTO> Handle(ComandoCalcularYGuardarEmparejamientos request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Iniciando cálculo de emparejamientos...");
        int emparejamientosCreados = 0;

        try
        {
            // 1. Obtener datos externos
            var vacantes = (await _fuenteVacantes.ObtenerVacantesAsync(cancellationToken)).ToList();
            var candidatos = (await _fuenteCandidatos.ObtenerCandidatosAsync(cancellationToken)).ToList();
            _logger.LogInformation("Obtenidas {VacantesCount} vacantes y {CandidatosCount} candidatos.", vacantes.Count, candidatos.Count);

            if (!vacantes.Any() || !candidatos.Any())
            {
                _logger.LogWarning("No hay suficientes datos para calcular emparejamientos.");
                return new ResultadoCalculoDTO(0);
            }

            // 2. Limpiar resultados anteriores (opcional pero común para recálculos)
            _logger.LogInformation("Limpiando emparejamientos anteriores...");
            await _repositorioEmparejamientos.LimpiarTodosAsync(cancellationToken);
            // Guardamos la limpieza antes de empezar a añadir nuevos
            await _unidadDeTrabajo.SaveChangesAsync(cancellationToken);
             _logger.LogInformation("Emparejamientos anteriores limpiados.");


            // 3. Calcular y guardar nuevos emparejamientos (N x M)
             _logger.LogInformation("Calculando nuevos emparejamientos con umbral {Umbral}...", request.UmbralPuntuacionMinima);
            foreach (var vacante in vacantes)
            {
                // Asegurarse que la vacante tenga ID (Mockaroo lo genera)
                if(vacante.IdExterno == Guid.Empty) continue;

                foreach (var candidato in candidatos)
                {
                     // Asegurarse que el candidato tenga email (nuestro ID temporal)
                    if(string.IsNullOrWhiteSpace(candidato.Email)) continue;

                    var puntuacion = _servicioEmparejamiento.CalcularPuntuacion(vacante, candidato);

                    if (puntuacion.PuntuacionTotal >= request.UmbralPuntuacionMinima)
                    {
                        var nuevoEmparejamiento = Emparejamiento.CrearNuevo(
                            vacante.IdExterno,
                            candidato.Email, // Usando email como ID!
                            puntuacion
                        );

                        await _repositorioEmparejamientos.AgregarAsync(nuevoEmparejamiento, cancellationToken);
                        emparejamientosCreados++;
                    }
                }
            }
            _logger.LogInformation("Cálculo finalizado. {EmparejamientosCreados} emparejamientos superaron el umbral.", emparejamientosCreados);


            // 4. Guardar todos los nuevos emparejamientos en la base de datos
            await _unidadDeTrabajo.SaveChangesAsync(cancellationToken);
            _logger.LogInformation("Nuevos emparejamientos guardados en la base de datos.");


            return new ResultadoCalculoDTO(emparejamientosCreados);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error durante el cálculo de emparejamientos.");
            // Podrías lanzar la excepción o devolver un resultado indicando el error
            // throw; // O:
            return new ResultadoCalculoDTO(-1); // Indicar error
        }
    }
}