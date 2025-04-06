using TesteMobi2Buy.Application.DTOs;
using TesteMobi2Buy.Domain.Entities;

namespace TesteMobi2Buy.Application.Interfaces;

public interface IClienteAppService
{
    Task<Cliente> CriarClienteAsync(CreateClienteDto dto);

    Task AtualizarClienteAsync(Guid id, UpdateClienteDto dto);

    Task<Cliente> ObterPorIdAsync(Guid id);
}
