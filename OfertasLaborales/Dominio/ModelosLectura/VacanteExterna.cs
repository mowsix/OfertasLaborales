using System.Text.Json.Serialization;

namespace Dominio.ModelosLectura;

public class VacanteExterna
{
    [JsonPropertyName("id_externo")]
    public Guid IdExterno { get; set; } 

    [JsonPropertyName("titulo")]
    public string? Titulo { get; set; } 

    [JsonPropertyName("departamento")]
    public string? Departamento { get; set; } 

    [JsonPropertyName("empresa_contratante")]
    public string? EmpresaContratante { get; set; } 

    [JsonPropertyName("fecha_publicacion")]
    public string? FechaPublicacion { get; set; } 

    [JsonPropertyName("ubicacion_pais")]
    public string? UbicacionPais { get; set; } 

    [JsonPropertyName("modalidad")]
    public string? Modalidad { get; set; } 

    [JsonPropertyName("posible_reubicacion")]
    public bool PosibleReubicacion { get; set; } 

    [JsonPropertyName("rango_salarial_min")]
    public decimal? RangoSalarialMin { get; set; } 

    [JsonPropertyName("rango_salarial_max")]
    public decimal? RangoSalarialMax { get; set; } 

    [JsonPropertyName("moneda")]
    public string? Moneda { get; set; } 
    [JsonPropertyName("area_experiencia_principal")]
    public string? AreaExperienciaPrincipal { get; set; } 

    [JsonPropertyName("tecnologia_principal")]
    public string? TecnologiaPrincipal { get; set; } 

    [JsonPropertyName("habilidad_blanda")]
    public string? HabilidadBlanda { get; set; } 

    [JsonPropertyName("idioma")]
    public string? Idioma { get; set; } 


}