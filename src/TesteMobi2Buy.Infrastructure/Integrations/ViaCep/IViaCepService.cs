namespace TesteMobi2Buy.Infrastructure.Integrations.ViaCep;

public interface IViaCepService
{
    Task<ViaCepResponse?> ConsultarCepAsync(string cep);
}
