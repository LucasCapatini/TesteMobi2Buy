using TesteMobi2Buy.Application.DTOs;
using TesteMobi2Buy.Application.Interfaces;
using TesteMobi2Buy.Domain.Entities;
using TesteMobi2Buy.Domain.Exceptions;
using TesteMobi2Buy.Domain.Interfaces;
using TesteMobi2Buy.Infrastructure.Integrations.ViaCep;
using TesteMobi2Buy.Infrastructure.Messaging.Events;
using TesteMobi2Buy.Infrastructure.Messaging.Interfaces;

namespace TesteMobi2Buy.Application.Services;

public class ClienteAppService : IClienteAppService
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IViaCepService _viaCepService;
    private readonly IClienteCadastradoProducer _clienteCadastradoProducer;

    public ClienteAppService(IClienteRepository clienteRepository, IViaCepService viaCepService, IClienteCadastradoProducer clienteCadastradoProducer)
    {
        _clienteRepository = clienteRepository;
        _viaCepService = viaCepService;
        _clienteCadastradoProducer = clienteCadastradoProducer;
    }

    public async Task<Cliente> CriarClienteAsync(CreateClienteDto dto)
    {
        var existente = await _clienteRepository.ObterPorEmailAsync(dto.Email);
        if (existente != null)
            throw new ClienteExistenteException(dto.Email);


        var endereco = await _viaCepService.ConsultarCepAsync(dto.Cep);
        if (endereco == null)
            throw new CepInvalidoException(dto.Cep);

        var cliente = new Cliente
        {
            Id = Guid.NewGuid(),
            Nome = dto.Nome,
            Email = dto.Email,
            Cep = dto.Cep,
            Logradouro = endereco.Logradouro,
            Bairro = endereco.Bairro,
            Cidade = endereco.Localidade,
            Estado = endereco.Uf
        };

        await _clienteRepository.AdicionarAsync(cliente);

        _clienteCadastradoProducer.Publicar("cliente_criado", new ClienteCriadoEvent
        {
            Nome = cliente.Nome,
            Email = cliente.Email
        });

        return cliente;
    }

    public async Task AtualizarClienteAsync(Guid id, UpdateClienteDto dto)
    {
        var cliente = await _clienteRepository
            .ObterPorIdAsync(id);

        if (cliente == null)
            throw new ClienteNaoEncontradoException(id);

        var clienteComEmail = await _clienteRepository
            .ObterPorEmailAsync(dto.Email);

        if (clienteComEmail != null && clienteComEmail.Email == dto.Email)
            throw new ClienteExistenteException(dto.Email);

        var endereco = await _viaCepService
            .ConsultarCepAsync(dto.Cep);

        if (endereco == null)
            throw new CepInvalidoException(dto.Cep);

        cliente.Nome = dto.Nome;
        cliente.Email = dto.Email;
        cliente.Cep = dto.Cep;
        cliente.Logradouro = endereco.Logradouro;
        cliente.Bairro = endereco.Bairro;
        cliente.Cidade = endereco.Localidade;
        cliente.Estado = endereco.Uf;

        await _clienteRepository.AtualizarAsync(cliente);
    }

    public async Task<Cliente> ObterPorIdAsync(Guid id)
    {
        var cliente = await _clienteRepository
            .ObterPorIdAsync(id);

        if (cliente == null)
            throw new ClienteNaoEncontradoException(id);

        return cliente;
    }

}
