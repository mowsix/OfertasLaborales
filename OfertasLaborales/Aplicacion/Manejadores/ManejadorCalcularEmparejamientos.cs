using Aplicacion.Comandos;
using Dominio.Agregados;
using Dominio.Interfaces;
using MediatR;

namespace Aplicacion.Manejadores;

public class ManejadorCalcularEmparejamientos : IRequestHandler<ComandoCalcularEmparejamientos, ResultadoCalculoEmparejamientos>
{
    private readonly IFuenteDatosExternaVacantes _fuenteVacantes;
    private readonly IFuenteDatosExternaCandidatos _fuenteCandidatos;
    private readonly IServicioEmparejamiento _servicioEmparejamiento;
    private readonly IRepositorioEmparejamientos _repositorioEmparejamientos;
    private readonly IUnidadDeTrabajo _unidadDeTrabajo;

    public ManejadorCalcularEmparejamientos(
        IFuenteDatosExternaVacantes fuenteVacantes,
        IFuenteDatosExternaCandidatos fuenteCandidatos,
        IServicioEmparejamiento servicioEmparejamiento,
        IRepositorioEmparejamientos repositorioEmparejamientos,
        IUnidadDeTrabajo unidadDeTrabajo)
    {
        _fuenteVacantes = fuenteVacantes;
        _fuenteCandidatos = fuenteCandidatos;
        _servicioEmparejamiento = servicioEmparejamiento;
        _repositorioEmparejamientos = repositorioEmparejamientos;
        _unidadDeTrabajo = unidadDeTrabajo;
    }

    public async Task<ResultadoCalculoEmparejamientos> Handle(ComandoCalcularEmparejamientos request, CancellationToken cancellationToken)
    {
        var resultado = new ResultadoCalculoEmparejamientos { Exito = false };

        try
        {
            // 1. Obtener datos de vacantes y candidatos
            var vacantes = await _fuenteVacantes.ObtenerVacantesAsync(cancellationToken);
            var candidatos = await _fuenteCandidatos.ObtenerCandidatosAsync(cancellationToken);

            resultado.TotalVacantes = vacantes.Count();
            resultado.TotalCandidatos = candidatos.Count();

            if (!vacantes.Any() || !candidatos.Any())
            {
                resultado.Mensaje = "No hay suficientes datos para realizar emparejamientos";
                return resultado;
            }

            // 2. Calcular emparejamientos
            var emparejamientos = await _servicioEmparejamiento.CalcularEmparejamientos(
                vacantes, candidatos, cancellationToken);

            // 3. Persistir resultados
            int contadorEmparejamientos = 0;

            foreach (var emparejamientoPorVacante in emparejamientos)
            {
                Guid idVacante = emparejamientoPorVacante.Key;

                foreach (var candidatoScore in emparejamientoPorVacante.Value)
                {
                    var emparejamiento = new Emparejamiento(
                        idVacante,
                        candidatoScore.Key,
                        candidatoScore.Value);

                    await _repositorioEmparejamientos.AgregarAsync(emparejamiento, cancellationToken);
                    contadorEmparejamientos++;
                }
            }

            // 4. Guardar todos los cambios
            await _unidadDeTrabajo.GuardarCambiosAsync(cancellationToken);

            resultado.EmparejamientosGenerados = contadorEmparejamientos;
            resultado.Exito = true;
            resultado.Mensaje = $"Se generaron {contadorEmparejamientos} emparejamientos exitosamente";
        }
        catch (Exception ex)
        {
            resultado.Mensaje = $"Error al calcular emparejamientos: {ex.Message}";
        }

        return resultado;
    }
}
