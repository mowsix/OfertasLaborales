using System.Text.Json.Serialization;

namespace Dominio.ModelosLectura;

public class CandidatoExterno
{
    // Se elimina la propiedad IdExterno según tu decisión

    [JsonPropertyName("nombre_completo")]
    public string? NombreCompleto { get; set; }

    [JsonPropertyName("email")]
    public string? Email { get; set; } // Este campo podría servir como identificador si es único

    [JsonPropertyName("telefono")]
    public string? Telefono { get; set; }

    [JsonPropertyName("ubicacion_pais")]
    public string? UbicacionPais { get; set; }

    [JsonPropertyName("disponibilidad_viajar")]
    public bool DisponibilidadViajar { get; set; }

    [JsonPropertyName("modalidades_preferidas")]
    public string? ModalidadesPreferidas { get; set; }

    [JsonPropertyName("expectativa_salarial_min")]
    public decimal? ExpectativaSalarialMin { get; set; }

    [JsonPropertyName("moneda_expectativa")]
    public string? MonedaExpectativa { get; set; }

    [JsonPropertyName("tecnologia_dominada")]
    public string? TecnologiaDominada { get; set; }

    [JsonPropertyName("habilidad_dominada")]
    public string? HabilidadDominada { get; set; }

    [JsonPropertyName("idioma_dominado")]
    public string? IdiomaDominado { get; set; }
}