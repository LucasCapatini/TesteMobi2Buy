using TesteMobi2Buy.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace TesteMobi2Buy.API.Configuration;

public static class DatabaseConfig
{
    public static void AddPostgreSqlConfiguration(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"),
                npgsqlOptionsAction: sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(Program).GetTypeInfo().Assembly.GetName().Name);
                    sqlOptions.EnableRetryOnFailure(maxRetryCount: 10, maxRetryDelay: TimeSpan.FromSeconds(30),
                        errorCodesToAdd: null);
                }
            ));
    }
}
