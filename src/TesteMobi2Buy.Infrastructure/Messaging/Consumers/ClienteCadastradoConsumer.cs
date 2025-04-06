using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using TesteMobi2Buy.Infrastructure.Messaging.Events;

namespace TesteMobi2Buy.Infrastructure.Messaging.Consumers;

public class ClienteCadastradoConsumer : BackgroundService
{
    private readonly ILogger<ClienteCadastradoConsumer> _logger;
    private IModel _channel;
    private IConnection _connection;
    public ClienteCadastradoConsumer(ILogger<ClienteCadastradoConsumer> logger)
    {
        _logger = logger;
    }

    public override void Dispose()
    {
        _channel?.Close();
        _connection?.Close();
        base.Dispose();
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var retryPolicy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(
                retryCount: 5,
                sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)),
                onRetry: (exception, timeSpan, retryCount, context) =>
                {
                    _logger
                    .LogWarning($"Tentativa {retryCount}: falha ao conectar no RabbitMQ. Tentando novamente em {timeSpan.TotalSeconds}s. Erro: {exception.Message}");
                });

        await retryPolicy.ExecuteAsync(async () =>
        {
            var factory = new ConnectionFactory
            {
                HostName = "rabbitmq",
                UserName = "guest",
                Password = "guest"
            };

            _logger.LogInformation("Tentando conectar ao RabbitMQ...");

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.QueueDeclare(
                queue: "cliente_criado",
                durable: false,
                exclusive: false,
                autoDelete: false);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var json = Encoding.UTF8.GetString(body);
                var cliente = JsonSerializer.Deserialize<ClienteCriadoEvent>(json);

                _logger.LogInformation("Mensagem recebida do RabbitMQ: {@Cliente}", cliente);
            };

            _channel.BasicConsume(
                queue: "cliente_criado",
                autoAck: true,
                consumer: consumer);

            _logger.LogInformation("Conectado ao RabbitMQ com sucesso e consumindo da fila 'cliente_criado'.");

            while (!stoppingToken.IsCancellationRequested)
            {
                await Task.Delay(1000, stoppingToken);
            }
        });
    }
}
