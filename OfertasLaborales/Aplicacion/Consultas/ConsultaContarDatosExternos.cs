using MediatR;
using Aplicacion.DTOs; // Asegúrate que el namespace sea correcto

namespace Aplicacion.Consultas;

// Esta consulta no necesita parámetros ahora, solo dispara la acción.
// Devuelve nuestro DTO de resumen.
public record ConsultaContarDatosExternos : IRequest<ResumenDatosExternosDTO>;