namespace Dominio.ObjetosValor;

// Usamos un record para inmutabilidad sencilla
public record PuntuacionEmparejamiento
{
    public int PuntuacionTotal { get; init; }
    // Podríamos añadir desglose si quisiéramos saber por qué se dio el puntaje
    // public Dictionary<string, int> DesglosePuntos { get; init; }

    // Constructor privado para asegurar creación controlada si es necesario
    private PuntuacionEmparejamiento(int puntuacionTotal)
    {
        PuntuacionTotal = puntuacionTotal;
        // DesglosePuntos = desglose ?? new Dictionary<string, int>();
    }

    // Fábrica estática para crear la puntuación
    public static PuntuacionEmparejamiento Crear(int puntuacionTotal /*, Dictionary<string, int>? desglose = null*/)
    {
        // Aquí podrías añadir validaciones si el puntaje debe estar en un rango, etc.
        return new PuntuacionEmparejamiento(puntuacionTotal /*, desglose*/);
    }
}