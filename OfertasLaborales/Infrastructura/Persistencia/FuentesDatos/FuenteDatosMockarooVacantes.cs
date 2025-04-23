using Microsoft.Extensions.Caching.Memory; // Para IMemoryCache
using System.Text.Json;
using Dominio.Interfaces;
using Dominio.ModelosLectura;
using Microsoft.Extensions.Logging; // Para Logging

namespace Infraestructura.FuentesDatos;

public class FuenteDatosMockarooVacantes : IFuenteDatosExternaVacantes
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache; // Inyectar caché
    private readonly ILogger<FuenteDatosMockarooVacantes> _logger; // Inyectar logger
    private const string CacheKeyVacantes = "ListaVacantesExternas";
    private const string MockarooUrl = "https://my.api.mockaroo.com/vacantes.json?key=a37864a0";

    public FuenteDatosMockarooVacantes(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache, // Añadir parámetro
        ILogger<FuenteDatosMockarooVacantes> logger) // Añadir parámetro
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache; // Asignar
        _logger = logger; // Asignar
    }

    public async Task<IEnumerable<VacanteExterna>> ObtenerVacantesAsync(CancellationToken cancellationToken = default)
    {
        // Intentar obtener del caché primero
        if (_cache.TryGetValue(CacheKeyVacantes, out IEnumerable<VacanteExterna>? vacantesCacheadas))
        {
            _logger.LogInformation("Datos de vacantes obtenidos desde caché.");
            // Asegurarse de no devolver null si el caché guardó null por alguna razón
            return vacantesCacheadas ?? Enumerable.Empty<VacanteExterna>();
        }

        // Si no está en caché, obtener de Mockaroo
        _logger.LogInformation("Cache de vacantes no encontrado. Obteniendo desde Mockaroo...");
        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(MockarooUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var vacantes = await JsonSerializer.DeserializeAsync<List<VacanteExterna>>(contentStream, options, cancellationToken);

            var resultado = vacantes ?? new List<VacanteExterna>();

            // Guardar en caché antes de devolver
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                // Mantener en caché por un tiempo determinado (ej. 1 hora)
                // Ajusta la duración según necesites
                .SetAbsoluteExpiration(TimeSpan.FromHours(1));

            _cache.Set(CacheKeyVacantes, resultado, cacheEntryOptions);
            _logger.LogInformation("Datos de vacantes guardados en caché.");

            return resultado;
        }
        catch (Exception e) // Captura más genérica para loguear cualquier error
        {
            _logger.LogError(e, "Error obteniendo o procesando datos de vacantes desde Mockaroo.");
            return Enumerable.Empty<VacanteExterna>(); // Devolver lista vacía en caso de error
        }
    }
}