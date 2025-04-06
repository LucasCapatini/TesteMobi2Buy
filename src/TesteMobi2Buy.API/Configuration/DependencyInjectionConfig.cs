using TesteMobi2Buy.Application.Interfaces;
using TesteMobi2Buy.Application.Services;
using TesteMobi2Buy.Domain.Interfaces;
using TesteMobi2Buy.Infrastructure.Data.Repositories;
using TesteMobi2Buy.Infrastructure.Integrations.ViaCep;
using TesteMobi2Buy.Infrastructure.Messaging.Consumers;
using TesteMobi2Buy.Infrastructure.Messaging.Interfaces;
using TesteMobi2Buy.Infrastructure.Messaging.Producers;

namespace TesteMobi2Buy.API.Configuration;

public static class DependencyInjectionConfig
{
    public static void AddDependencyInjectionConfiguration(this IServiceCollection services)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddHttpClient<IViaCepService, ViaCepService>();
        services.AddScoped<IClienteAppService, ClienteAppService>();

        services.AddSingleton<IClienteCadastradoProducer, ClienteCadastradoProducer>();
        services.AddHostedService<ClienteCadastradoConsumer>();
    }
}
