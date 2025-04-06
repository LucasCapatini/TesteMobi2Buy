using TesteMobi2Buy.Domain.Entities;

namespace TesteMobi2Buy.Domain.Interfaces;

public interface IClienteRepository
{
    Task<Cliente?> ObterPorEmailAsync(string email);
    Task<Cliente?> ObterPorIdAsync(Guid id);
    Task AdicionarAsync(Cliente cliente);
    Task AtualizarAsync(Cliente cliente);
}
