namespace TesteMobi2Buy.Domain.Exceptions;

public class ClienteNaoEncontradoException : Exception
{
    public ClienteNaoEncontradoException(Guid id) : base($"O cliente com o ID '{id}' não foi encontrado.") { }
}
