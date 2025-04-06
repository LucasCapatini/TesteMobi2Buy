namespace TesteMobi2Buy.Domain.Exceptions;

public class CepInvalidoException : Exception
{
    public CepInvalidoException(string cep) : base($"O CEP '{cep}' é inválido.") { }
}
