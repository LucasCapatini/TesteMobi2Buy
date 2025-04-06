namespace TesteMobi2Buy.Infrastructure.Messaging.Interfaces;

public interface IClienteCadastradoProducer
{
    void Publicar<T>(string queue, T message);
}
