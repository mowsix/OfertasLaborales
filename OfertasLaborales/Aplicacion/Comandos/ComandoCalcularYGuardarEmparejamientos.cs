// Dispara el proceso de cálculo de emparejamientos.
using MediatR;
using Aplicacion.DTOs; // Para ResultadoCalculoDTO

namespace Aplicacion.Comandos;

// Umbral: Puntuación mínima para guardar el emparejamiento
public record ComandoCalcularYGuardarEmparejamientos(int UmbralPuntuacionMinima) : IRequest<ResultadoCalculoDTO>;
