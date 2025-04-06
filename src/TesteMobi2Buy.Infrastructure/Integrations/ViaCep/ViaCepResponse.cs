namespace TesteMobi2Buy.Infrastructure.Integrations.ViaCep;

public class ViaCepResponse
{
    public string Cep { get; set; } = null!;
    public string Logradouro { get; set; } = null!;
    public string Bairro { get; set; } = null!;
    public string Localidade { get; set; } = null!;
    public string Uf { get; set; } = null!;
    public bool Erro { get; set; } = false;
}
