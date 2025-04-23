 // Representa el resultado (score, estado) del cruce entre un IdVacante y un IdCandidato.
 using Dominio.ObjetosValor; // Para PuntuacionEmparejamiento

namespace Dominio.Agregados;

public class Emparejamiento
{
    public Guid Id { get; private set; } // ID propio del emparejamiento
    public Guid IdVacanteExterna { get; private set; } // ID de la vacante (del JSON)
    public string IdCandidatoExterno { get; private set; } // Email del candidato (del JSON)
    public PuntuacionEmparejamiento Puntuacion { get; private set; }
    public DateTime FechaCalculo { get; private set; }

    // Constructor protegido para EF Core y privado para el uso interno/factorías
    protected Emparejamiento() {
         // Requerido por EF Core si se usa constructor privado/protegido
         IdCandidatoExterno = string.Empty; // Inicializar para evitar advertencias de nulabilidad
         Puntuacion = PuntuacionEmparejamiento.Crear(0); // Valor por defecto
    }

    // Constructor para crear nuevos emparejamientos
    private Emparejamiento(Guid idVacanteExterna, string idCandidatoExterno, PuntuacionEmparejamiento puntuacion)
    {
        // Validaciones básicas
        if (idVacanteExterna == Guid.Empty)
            throw new ArgumentException("El ID de vacante no puede estar vacío.", nameof(idVacanteExterna));
        if (string.IsNullOrWhiteSpace(idCandidatoExterno))
            throw new ArgumentException("El ID (email) de candidato no puede estar vacío.", nameof(idCandidatoExterno));

        Id = Guid.NewGuid(); // Generamos un nuevo ID único para este emparejamiento
        IdVacanteExterna = idVacanteExterna;
        IdCandidatoExterno = idCandidatoExterno;
        Puntuacion = puntuacion ?? throw new ArgumentNullException(nameof(puntuacion));
        FechaCalculo = DateTime.UtcNow; // Guardamos la fecha del cálculo en UTC
    }

    // Método Fábrica Estático (preferido para crear agregados)
    public static Emparejamiento CrearNuevo(Guid idVacanteExterna, string idCandidatoExterno, PuntuacionEmparejamiento puntuacion)
    {
        return new Emparejamiento(idVacanteExterna, idCandidatoExterno, puntuacion);
    }

    // Podríamos añadir métodos para actualizar estado si fuera necesario (ej. marcar como notificado)
}