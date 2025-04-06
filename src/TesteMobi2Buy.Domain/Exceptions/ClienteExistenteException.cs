namespace TesteMobi2Buy.Domain.Exceptions;

public class ClienteExistenteException : Exception
{
    public ClienteExistenteException(string email) : base($"O cliente com o e-mail '{email}' já está cadastrado.") { }
}
