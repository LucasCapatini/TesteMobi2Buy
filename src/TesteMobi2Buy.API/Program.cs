using Microsoft.EntityFrameworkCore;
using System.Text.Encodings.Web;
using System.Text.Json;
using TesteMobi2Buy.API.Configuration;
using TesteMobi2Buy.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

try
{
    builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

    builder.Services.AddPostgreSqlConfiguration(builder.Configuration);

    builder.Services.AddDependencyInjectionConfiguration();

    builder.Services.AddSwaggerConfiguration();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.MapControllers();

    using (var scope = app.Services.CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        dbContext.Database.Migrate();
    }

    app.Run();
}
catch (Exception e)
{
    throw new Exception("Ocorreu um erro ao configurar a aplicação.", e);
}