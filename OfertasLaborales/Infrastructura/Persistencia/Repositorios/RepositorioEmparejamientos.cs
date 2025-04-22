using Dominio.Agregados;
using Dominio.Interfaces;
using Microsoft.EntityFrameworkCore;
using Infraestructura.Persistencia.ContextoDB;

namespace Infraestructura.Persistencia.Repositorios;

public class RepositorioEmparejamientos : IRepositorioEmparejamientos
{
    private readonly ContextoBdEmparejamiento _contexto;

    public RepositorioEmparejamientos(ContextoBdEmparejamiento contexto)
    {
        _contexto = contexto;
    }

    public async Task<Emparejamiento?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _contexto.Emparejamientos
            .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerPorVacanteAsync(Guid idVacante, CancellationToken cancellationToken = default)
    {
        return await _contexto.Emparejamientos
            .Where(e => e.IdVacante == idVacante)
            .OrderByDescending(e => e.Puntuacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerPorCandidatoAsync(string emailCandidato, CancellationToken cancellationToken = default)
    {
        return await _contexto.Emparejamientos
            .Where(e => e.EmailCandidato == emailCandidato)
            .OrderByDescending(e => e.Puntuacion)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<Emparejamiento>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await _contexto.Emparejamientos
            .OrderByDescending(e => e.Puntuacion)
            .ToListAsync(cancellationToken);
    }

    public async Task AgregarAsync(Emparejamiento emparejamiento, CancellationToken cancellationToken = default)
    {
        await _contexto.Emparejamientos.AddAsync(emparejamiento, cancellationToken);
    }

    public void Actualizar(Emparejamiento emparejamiento)
    {
        _contexto.Emparejamientos.Update(emparejamiento);
    }

    public void Eliminar(Emparejamiento emparejamiento)
    {
        _contexto.Emparejamientos.Remove(emparejamiento);
    }
}