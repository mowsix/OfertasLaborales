using MediatR;

namespace Aplicacion.Comandos;

// Comando simple para disparar el proceso de cálculo
public record ComandoCalcularEmparejamientos : IRequest<ResultadoCalculoEmparejamientos>;

// Clase para devolver resultados del cálculo
public class ResultadoCalculoEmparejamientos
{
    public int TotalVacantes { get; set; }
    public int TotalCandidatos { get; set; }
    public int EmparejamientosGenerados { get; set; }
    public bool Exito { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}


