namespace Aplicacion.DTOs;

public class EmparejamientoDetalleDTO
{
    public Guid IdEmparejamiento { get; set; } // El ID del registro Emparejamiento
    public Guid IdVacanteExterna { get; set; }
    public string IdCandidatoExterno { get; set; } = string.Empty; // Email
    public int PuntuacionTotal { get; set; }
    public DateTime FechaCalculo { get; set; }

    // Podríamos añadir más info aquí (título vacante, nombre candidato) si hacemos el enriquecimiento
    // public string? TituloVacante { get; set; }
    // public string? NombreCandidato { get; set; }
}