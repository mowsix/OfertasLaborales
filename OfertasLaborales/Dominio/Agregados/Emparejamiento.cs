using System;

namespace Dominio.Agregados;

public class Emparejamiento
{
    public int Id { get; private set; }
    public Guid IdVacante { get; private set; }
    public string EmailCandidato { get; private set; }
    public double Puntuacion { get; private set; }
    public DateTime FechaCalculo { get; private set; }
    public string Estado { get; private set; } // Pendiente, Notificado, Descartado

    // Constructor privado para EF Core
    private Emparejamiento() { }

    public Emparejamiento(Guid idVacante, string emailCandidato, double puntuacion)
    {
        if (idVacante == Guid.Empty)
            throw new ArgumentException("El ID de la vacante no puede estar vacío");

        if (string.IsNullOrWhiteSpace(emailCandidato))
            throw new ArgumentException("El email del candidato no puede estar vacío");

        if (puntuacion < 0 || puntuacion > 100)
            throw new ArgumentException("La puntuación debe estar entre 0 y 100");

        IdVacante = idVacante;
        EmailCandidato = emailCandidato;
        Puntuacion = puntuacion;
        FechaCalculo = DateTime.UtcNow;
        Estado = "Pendiente";
    }

    public void MarcarComoNotificado()
    {
        Estado = "Notificado";
    }

    public void Descartar()
    {
        Estado = "Descartado";
    }
}