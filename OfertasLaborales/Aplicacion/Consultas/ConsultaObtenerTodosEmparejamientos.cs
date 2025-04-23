using MediatR;
using Aplicacion.DTOs; // Para EmparejamientoDetalleDTO

namespace Aplicacion.Consultas;

public record ConsultaObtenerTodosEmparejamientos : IRequest<IEnumerable<EmparejamientoDetalleDTO>>;