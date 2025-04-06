using System.Text.Json;
using System.Text;
using TesteMobi2Buy.Infrastructure.Messaging.Interfaces;
using RabbitMQ.Client;

namespace TesteMobi2Buy.Infrastructure.Messaging.Producers;

public class ClienteCadastradoProducer : IClienteCadastradoProducer
{
    private readonly IConnection _connection;

    public ClienteCadastradoProducer()
    {
        var factory = new ConnectionFactory()
        {
            HostName = "rabbitmq",
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
    }

    public void Publicar<T>(string queue, T message)
    {
        using var channel = _connection.CreateModel();

        channel.QueueDeclare(queue, durable: false, exclusive: false, autoDelete: false);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        channel.BasicPublish("", queue, null, body);
    }
}
