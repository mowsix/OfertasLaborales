 // Contrato para la lógica de cálculo de emparejamiento.
using Dominio.ModelosLectura;
using Dominio.ObjetosValor;

namespace Dominio.Interfaces;

public interface IServicioEmparejamiento
{
    PuntuacionEmparejamiento CalcularPuntuacion(VacanteExterna vacante, CandidatoExterno candidato);
}