using Dominio.Interfaces;
using Infraestructura.Persistencia.ContextoDB;

namespace Infraestructura.Persistencia.UnidadDeTrabajo;

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly ContextoBdEmparejamiento _contexto;

    public UnidadDeTrabajo(ContextoBdEmparejamiento contexto)
    {
        _contexto = contexto;
    }

    public async Task<int> GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        return await _contexto.SaveChangesAsync(cancellationToken);
    }
}