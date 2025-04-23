 // Implementa IUnidadDeTrabajo (opera sobre ContextoBdEmparejamiento).
using Dominio.Interfaces; // Para IUnidadDeTrabajo
using Infraestructura.Persistencia; // Para ContextoBdEmparejamiento

namespace Infraestructura.Persistencia; // Asegúrate que el namespace sea correcto

public class UnidadDeTrabajo : IUnidadDeTrabajo
{
    private readonly ContextoBdEmparejamiento _contexto;
    private bool _disposed = false; // Para el patrón IDisposable

    // Inyectamos el DbContext
    public UnidadDeTrabajo(ContextoBdEmparejamiento contexto)
    {
        _contexto = contexto ?? throw new ArgumentNullException(nameof(contexto));
    }

    // Implementación del método principal
    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await _contexto.SaveChangesAsync(cancellationToken);
    }

    // Implementación de IDisposable (importante para liberar el DbContext)
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this); // Notifica al recolector de basura que no llame al finalizador
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                // Liberar recursos manejados (el DbContext)
                _contexto.Dispose();
            }
            // Liberar recursos no manejados (si los hubiera)
            _disposed = true;
        }
    }

    // Destructor (finalizador) - como fallback por si no se llama a Dispose()
    // No es estrictamente necesario si siempre se usa 'using' o DI con scope,
    // pero es parte del patrón completo de IDisposable.
    ~UnidadDeTrabajo()
    {
       Dispose(false);
    }
}