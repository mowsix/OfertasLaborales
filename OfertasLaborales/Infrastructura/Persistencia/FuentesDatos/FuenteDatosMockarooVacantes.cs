using System.Text.Json;
using Dominio.Interfaces;
using Dominio.ModelosLectura;

namespace Infraestructura.FuentesDatos;

public class FuenteDatosMockarooVacantes : IFuenteDatosExternaVacantes
{
    private readonly IHttpClientFactory _httpClientFactory;
    // Guarda la URL en una constante o léela de configuración más adelante
    private const string MockarooUrl = "https://my.api.mockaroo.com/vacantes.json?key=a37864a0";

    public FuenteDatosMockarooVacantes(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<VacanteExterna>> ObtenerVacantesAsync(CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(MockarooUrl, cancellationToken);
            response.EnsureSuccessStatusCode(); // Lanza excepción si no es 2xx

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);

            // Opciones para deserializar (puedes necesitarlas si nombres no coinciden o para fechas)
            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Facilita si no usaste [JsonPropertyName] exacto
            };

            var vacantes = await JsonSerializer.DeserializeAsync<List<VacanteExterna>>(contentStream, options, cancellationToken);

            return vacantes ?? new List<VacanteExterna>();
        }
        catch (HttpRequestException e)
        {
            // Loguear el error aquí (usando ILogger inyectado más adelante)
            Console.WriteLine($"Error HTTP obteniendo vacantes: {e.Message}");
            return new List<VacanteExterna>(); // O lanzar una excepción personalizada
        }
        catch (JsonException e)
        {
            // Loguear el error aquí
            Console.WriteLine($"Error deserializando vacantes: {e.Message}");
            return new List<VacanteExterna>(); // O lanzar una excepción personalizada
        }
    }
}