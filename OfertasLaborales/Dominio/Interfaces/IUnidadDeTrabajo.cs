// Contrato para la Unidad de Trabajo (opera sobre repositorios propios).
namespace Dominio.Interfaces;

public interface IUnidadDeTrabajo : IDisposable // Es buena práctica que sea IDisposable
{
    /// <summary>
    /// Guarda todos los cambios realizados en el contexto de la base de datos.
    /// </summary>
    /// <param name="cancellationToken">Token de cancelación.</param>
    /// <returns>El número de objetos de estado escritos en la base de datos.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    // Opcional: Podrías añadir aquí métodos para obtener los repositorios si
    // quisieras que la Unidad de Trabajo actúe como una fachada para ellos,
    // pero el enfoque actual de inyectar repositorios directamente en los
    // manejadores también es válido y común. Por ahora, solo necesitamos SaveChangesAsync.
    // Ejemplo opcional: IRepositorioEmparejamientos Emparejamientos { get; }
}