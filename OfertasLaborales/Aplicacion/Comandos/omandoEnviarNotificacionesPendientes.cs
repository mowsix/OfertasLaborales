using MediatR;
using Aplicacion.DTOs; // Para ResultadoNotificacionDTO

namespace Aplicacion.Comandos;

// No necesita parámetros, dispara el proceso para todas las pendientes
public record ComandoEnviarNotificacionesPendientes : IRequest<ResultadoNotificacionDTO>;