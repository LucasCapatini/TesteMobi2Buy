namespace TesteMobi2Buy.Domain.Entities;

public class Cliente
{
    public Guid Id { get; set; }
    public string Nome { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Cep { get; set; } = null!;
    public string Logradouro { get; set; } = null!;
    public string Bairro { get; set; } = null!;
    public string Cidade { get; set; } = null!;
    public string Estado { get; set; } = null!;
}