using System.Net.Http.Json;

namespace TesteMobi2Buy.Infrastructure.Integrations.ViaCep;

public class ViaCepService : IViaCepService
{
    private readonly HttpClient _httpClient;

    public ViaCepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ViaCepResponse?> ConsultarCepAsync(string cep)
    {
        if (string.IsNullOrWhiteSpace(cep)) 
            return null;

        try
        {
            var response = await _httpClient.GetFromJsonAsync<ViaCepResponse>($"https://viacep.com.br/ws/{cep}/json/");

            if (response == null || response.Erro)
                return null;

            return response;
        }
        catch
        {
            return null;
        }
    }
}
