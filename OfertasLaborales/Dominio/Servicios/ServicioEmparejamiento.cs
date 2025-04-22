using Dominio.Interfaces;
using Dominio.ModelosLectura;
namespace Dominio.Servicios;
public class ServicioEmparejamiento : IServicioEmparejamiento
{
    // Implementación sencilla que calcula puntuaciones basadas en criterios de emparejamiento
    public async Task<Dictionary<Guid, Dictionary<string, double>>> CalcularEmparejamientos(
        IEnumerable<VacanteExterna> vacantes,
        IEnumerable<CandidatoExterno> candidatos,
        CancellationToken cancellationToken = default)
    {
        var resultados = new Dictionary<Guid, Dictionary<string, double>>();
        foreach (var vacante in vacantes)
        {
            var emparejamientosPorVacante = new Dictionary<string, double>();
            foreach (var candidato in candidatos)
            {
                if (candidato.Email == null) continue; // Saltamos candidatos sin identificador
                // Calculamos puntuación usando varios criterios
                double puntuacion = 0;

                // 1. Ubicación geográfica (25%)
                if (string.Equals(vacante.UbicacionPais, candidato.UbicacionPais, StringComparison.OrdinalIgnoreCase))
                {
                    puntuacion += 25;
                }
                else if (vacante.PosibleReubicacion && candidato.DisponibilidadViajar)
                {
                    puntuacion += 12.5; // Medio puntaje si hay posibilidad de reubicación y disponibilidad
                }

                // 2. Tecnología/habilidades (30%)
                if (!string.IsNullOrEmpty(vacante.TecnologiaPrincipal) &&
                    !string.IsNullOrEmpty(candidato.TecnologiaDominada) &&
                    string.Equals(vacante.TecnologiaPrincipal, candidato.TecnologiaDominada, StringComparison.OrdinalIgnoreCase))
                {
                    puntuacion += 30;
                }

                // 3. Idioma (15%) - Nuevo criterio
                if (!string.IsNullOrEmpty(vacante.Idioma) &&
                    !string.IsNullOrEmpty(candidato.IdiomaDominado))
                {
                    if (candidato.IdiomaDominado.Contains(vacante.Idioma, StringComparison.OrdinalIgnoreCase))
                    {
                        puntuacion += 15;
                    }
                }

                // 4. Modalidad de trabajo (15%)
                if (!string.IsNullOrEmpty(vacante.Modalidad) &&
                    !string.IsNullOrEmpty(candidato.ModalidadesPreferidas) &&
                    candidato.ModalidadesPreferidas.Contains(vacante.Modalidad, StringComparison.OrdinalIgnoreCase))
                {
                    puntuacion += 15;
                }

                // 5. Compatibilidad salarial (15%)
                if (vacante.RangoSalarialMax.HasValue && candidato.ExpectativaSalarialMin.HasValue)
                {
                    if (vacante.RangoSalarialMax.Value >= candidato.ExpectativaSalarialMin.Value &&
                        string.Equals(vacante.Moneda, candidato.MonedaExpectativa, StringComparison.OrdinalIgnoreCase))
                    {
                        puntuacion += 15;
                    }
                }

                // Solo guardamos emparejamientos con puntuación mínima de 50 puntos
                if (puntuacion >= 70)
                {
                    emparejamientosPorVacante[candidato.Email] = puntuacion;
                }
            }
            // Guardamos los emparejamientos encontrados para esta vacante
            if (emparejamientosPorVacante.Count > 0)
            {
                resultados[vacante.IdExterno] = emparejamientosPorVacante;
            }
        }
        return resultados;
    }
}