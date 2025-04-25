// Implementa IRepositorioNotificacionesEmparejamiento (guarda/lee en BD propia).
using Microsoft.EntityFrameworkCore;
using Dominio.Agregados;
using Dominio.Interfaces;
using Dominio.ObjetosValor;
using Infraestructura.Persistencia;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioNotificacionesEmparejamiento : IRepositorioNotificacionesEmparejamiento
{
    private readonly ContextoBdEmparejamiento _contexto;

    public RepositorioNotificacionesEmparejamiento(ContextoBdEmparejamiento contexto)
    {
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
    }

    public async Task AgregarAsync(NotificacionEmparejamiento notificacion, CancellationToken cancellationToken = default)
    {
        await _contexto.NotificacionesEmparejamiento.AddAsync(notificacion, cancellationToken);
    }

    public async Task<NotificacionEmparejamiento?> ObtenerPorIdEmparejamientoAsync(Guid idEmparejamiento, CancellationToken cancellationToken = default)
    {
        return await _contexto.NotificacionesEmparejamiento
                              .FirstOrDefaultAsync(n => n.IdEmparejamiento == idEmparejamiento, cancellationToken);
    }

     public async Task<IEnumerable<NotificacionEmparejamiento>> ObtenerPorEstadoAsync(EstadoNotificacion estado, CancellationToken cancellationToken = default)
    {
         return await _contexto.NotificacionesEmparejamiento
                               .Where(n => n.Estado == estado)
                               .ToListAsync(cancellationToken);
    }

     public async Task<IEnumerable<NotificacionEmparejamiento>> ObtenerEnviadasAsync(CancellationToken cancellationToken = default)
    {
         // Devuelve solo las que se marcaron como enviadas exitosamente
          return await _contexto.NotificacionesEmparejamiento
                               .Where(n => n.Estado == EstadoNotificacion.Enviada)
                               .ToListAsync(cancellationToken);
    }

    // Si necesitaras un Update explícito (aunque SaveChangesAsync de UoW debería bastar)
    // public Task ActualizarAsync(NotificacionEmparejamiento notificacion, CancellationToken cancellationToken = default)
    // {
    //     _contexto.Entry(notificacion).State = EntityState.Modified;
    //     return Task.CompletedTask;
    // }
}