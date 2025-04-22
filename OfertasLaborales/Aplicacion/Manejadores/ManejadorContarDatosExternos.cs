using MediatR;
using Aplicacion.Consultas;
using Aplicacion.DTOs;
using Dominio.Interfaces; // Para las fuentes de datos

namespace Aplicacion.Manejadores;

public class ManejadorContarDatosExternos : IRequestHandler<ConsultaContarDatosExternos, ResumenDatosExternosDTO>
{
    private readonly IFuenteDatosExternaVacantes _fuenteVacantes;
    private readonly IFuenteDatosExternaCandidatos _fuenteCandidatos;

    public ManejadorContarDatosExternos(
        IFuenteDatosExternaVacantes fuenteVacantes,
        IFuenteDatosExternaCandidatos fuenteCandidatos)
    {
        _fuenteVacantes = fuenteVacantes;
        _fuenteCandidatos = fuenteCandidatos;
    }

    public async Task<ResumenDatosExternosDTO> Handle(ConsultaContarDatosExternos request, CancellationToken cancellationToken)
    {
        var resumen = new ResumenDatosExternosDTO();
        string mensajeError = "";

        try
        {
            var vacantes = await _fuenteVacantes.ObtenerVacantesAsync(cancellationToken);
            resumen.NumeroVacantes = vacantes.Count();
        }
        catch (Exception ex) // Captura general por simplicidad inicial
        {
             Console.WriteLine($"Error al obtener vacantes: {ex.Message}");
             mensajeError += "Error al obtener vacantes. ";
             resumen.NumeroVacantes = -1; // Indicar error
        }

         try
        {
            var candidatos = await _fuenteCandidatos.ObtenerCandidatosAsync(cancellationToken);
            resumen.NumeroCandidatos = candidatos.Count();
        }
        catch (Exception ex) // Captura general por simplicidad inicial
        {
             Console.WriteLine($"Error al obtener candidatos: {ex.Message}");
             mensajeError += "Error al obtener candidatos.";
             resumen.NumeroCandidatos = -1; // Indicar error
        }

        resumen.Mensaje = string.IsNullOrEmpty(mensajeError) ? "Datos externos leídos." : mensajeError;

        return resumen;
    }
}