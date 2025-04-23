using Microsoft.Extensions.Caching.Memory; // Para IMemoryCache
using System.Text.Json;
using Dominio.Interfaces;
using Dominio.ModelosLectura;
using Microsoft.Extensions.Logging; // Para Logging

namespace Infraestructura.FuentesDatos;

public class FuenteDatosMockarooCandidatos : IFuenteDatosExternaCandidatos
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IMemoryCache _cache; // Inyectar caché
    private readonly ILogger<FuenteDatosMockarooCandidatos> _logger; // Inyectar logger
    private const string CacheKeyCandidatos = "ListaCandidatosExternos";
    private const string MockarooUrl = "https://my.api.mockaroo.com/candidatos.json?key=a37864a0";

    public FuenteDatosMockarooCandidatos(
        IHttpClientFactory httpClientFactory,
        IMemoryCache cache, // Añadir parámetro
        ILogger<FuenteDatosMockarooCandidatos> logger) // Añadir parámetro
    {
        _httpClientFactory = httpClientFactory;
        _cache = cache; // Asignar
        _logger = logger; // Asignar
    }

    public async Task<IEnumerable<CandidatoExterno>> ObtenerCandidatosAsync(CancellationToken cancellationToken = default)
    {
         // Intentar obtener del caché primero
        if (_cache.TryGetValue(CacheKeyCandidatos, out IEnumerable<CandidatoExterno>? candidatosCacheados))
        {
            _logger.LogInformation("Datos de candidatos obtenidos desde caché.");
            return candidatosCacheados ?? Enumerable.Empty<CandidatoExterno>();
        }

         // Si no está en caché, obtener de Mockaroo
        _logger.LogInformation("Cache de candidatos no encontrado. Obteniendo desde Mockaroo...");
        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(MockarooUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var candidatos = await JsonSerializer.DeserializeAsync<List<CandidatoExterno>>(contentStream, options, cancellationToken);

            var resultado = candidatos ?? new List<CandidatoExterno>();

            // Guardar en caché
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetAbsoluteExpiration(TimeSpan.FromHours(1)); // Cache por 1 hora

            _cache.Set(CacheKeyCandidatos, resultado, cacheEntryOptions);
             _logger.LogInformation("Datos de candidatos guardados en caché.");

            return resultado;
        }
        catch (Exception e)
        {
             _logger.LogError(e, "Error obteniendo o procesando datos de candidatos desde Mockaroo.");
            return Enumerable.Empty<CandidatoExterno>();
        }
    }
}