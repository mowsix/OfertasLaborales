using System.Text.Json;
using Dominio.Interfaces;
using Dominio.ModelosLectura;

namespace Infraestructura.FuentesDatos;

public class FuenteDatosMockarooCandidatos : IFuenteDatosExternaCandidatos
{
    private readonly IHttpClientFactory _httpClientFactory;
    private const string MockarooUrl = "https://my.api.mockaroo.com/candidatos.json?key=a37864a0";

    public FuenteDatosMockarooCandidatos(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<CandidatoExterno>> ObtenerCandidatosAsync(CancellationToken cancellationToken = default)
    {
        var httpClient = _httpClientFactory.CreateClient();
        try
        {
            var response = await httpClient.GetAsync(MockarooUrl, cancellationToken);
            response.EnsureSuccessStatusCode();

            await using var contentStream = await response.Content.ReadAsStreamAsync(cancellationToken);
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var candidatos = await JsonSerializer.DeserializeAsync<List<CandidatoExterno>>(contentStream, options, cancellationToken);

            return candidatos ?? new List<CandidatoExterno>();
        }
        catch (HttpRequestException e)
        {
             Console.WriteLine($"Error HTTP obteniendo candidatos: {e.Message}");
            return new List<CandidatoExterno>();
        }
        catch (JsonException e)
        {
             Console.WriteLine($"Error deserializando candidatos: {e.Message}");
            return new List<CandidatoExterno>();
        }
    }
}