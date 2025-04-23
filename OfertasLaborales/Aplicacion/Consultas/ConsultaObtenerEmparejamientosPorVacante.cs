// Obtiene los candidatos emparejados con una vacante específica.
using MediatR;
using Aplicacion.DTOs;

namespace Aplicacion.Consultas;

// Consulta para obtener los candidatos emparejados con una vacante específica
public record ConsultaObtenerEmparejamientosPorVacante(Guid IdVacante) : IRequest<IEnumerable<EmparejamientoDetalleDTO>>;