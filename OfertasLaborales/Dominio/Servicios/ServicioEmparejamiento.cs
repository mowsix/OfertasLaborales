// Implementa el cálculo de compatibilidad (score).
using Dominio.Interfaces;
using Dominio.ModelosLectura;
using Dominio.ObjetosValor;

namespace Dominio.Servicios;

public class ServicioEmparejamiento : IServicioEmparejamiento
{
    // Define aquí los puntos para cada criterio de match
    private const int PUNTOS_IDIOMA = 25;
    private const int PUNTOS_TECNOLOGIA = 25;
    private const int PUNTOS_HABILIDAD = 10;
    private const int PUNTOS_MODALIDAD = 10;
    private const int PUNTOS_SALARIO = 10;
    private const int PUNTOS_UBICACION = 10;

    public PuntuacionEmparejamiento CalcularPuntuacion(VacanteExterna vacante, CandidatoExterno candidato)
    {
        int puntuacion = 0;

        // --- Comparaciones (ignorar mayúsculas/minúsculas y espacios) ---

        // 1. Idioma
        if (!string.IsNullOrWhiteSpace(vacante.Idioma) &&
            vacante.Idioma.Equals(candidato.IdiomaDominado, StringComparison.OrdinalIgnoreCase))
        {
            puntuacion += PUNTOS_IDIOMA;
        }

        // 2. Tecnología Principal
        if (!string.IsNullOrWhiteSpace(vacante.TecnologiaPrincipal) &&
            vacante.TecnologiaPrincipal.Equals(candidato.TecnologiaDominada, StringComparison.OrdinalIgnoreCase))
        {
            puntuacion += PUNTOS_TECNOLOGIA;
        }

        // 3. Habilidad Blanda/Dominada
        if (!string.IsNullOrWhiteSpace(vacante.HabilidadBlanda) &&
            vacante.HabilidadBlanda.Equals(candidato.HabilidadDominada, StringComparison.OrdinalIgnoreCase))
        {
            puntuacion += PUNTOS_HABILIDAD;
        }

        // 4. Modalidad
        if (!string.IsNullOrWhiteSpace(vacante.Modalidad) &&
            vacante.Modalidad.Equals(candidato.ModalidadesPreferidas, StringComparison.OrdinalIgnoreCase))
        {
            puntuacion += PUNTOS_MODALIDAD;
        }
        // Podríamos dar puntos parciales si la vacante es Hibrida y el candidato prefiere Remoto o viceversa? (Por ahora, match exacto)

        // 5. Salario (Asegurarse que ambos tengan valores y misma moneda)
        if (candidato.ExpectativaSalarialMin.HasValue &&
            vacante.RangoSalarialMin.HasValue &&
            vacante.RangoSalarialMax.HasValue &&
            !string.IsNullOrWhiteSpace(vacante.Moneda) &&
            vacante.Moneda.Equals(candidato.MonedaExpectativa, StringComparison.OrdinalIgnoreCase))
        {
            // Candidato pide algo dentro del rango de la vacante
            if (candidato.ExpectativaSalarialMin >= vacante.RangoSalarialMin &&
                candidato.ExpectativaSalarialMin <= vacante.RangoSalarialMax)
            {
                puntuacion += PUNTOS_SALARIO;
            }
            // Podríamos añadir lógica para puntuar más si pide menos, etc.
        }

        // 6. Ubicación (Simplificado: Coincide país O la vacante es Remota)
        bool ubicacionCoincide = false;
        if (!string.IsNullOrWhiteSpace(vacante.UbicacionPais) &&
            vacante.UbicacionPais.Equals(candidato.UbicacionPais, StringComparison.OrdinalIgnoreCase))
        {
            ubicacionCoincide = true;
        }
         if (!string.IsNullOrWhiteSpace(vacante.Modalidad) &&
             vacante.Modalidad.Equals("Remoto", StringComparison.OrdinalIgnoreCase))
        {
             // Si la vacante es remota, la ubicación del candidato no importa tanto para este criterio
             ubicacionCoincide = true;
             // Podríamos no sumar puntos o sumar menos si el candidato prefiere presencial? Por ahora lo simplificamos.
        }
        // Falta considerar la reubicación (necesitaría el campo en Candidato)

        if (ubicacionCoincide)
        {
            puntuacion += PUNTOS_UBICACION;
        }

        // --- Crear y devolver el objeto Puntuacion ---
        return PuntuacionEmparejamiento.Crear(puntuacion);
    }
}