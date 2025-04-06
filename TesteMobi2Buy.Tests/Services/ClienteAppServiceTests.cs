using Moq;
using TesteMobi2Buy.Application.DTOs;
using TesteMobi2Buy.Application.Services;
using TesteMobi2Buy.Domain.Entities;
using TesteMobi2Buy.Domain.Interfaces;
using TesteMobi2Buy.Infrastructure.Integrations.ViaCep;
using TesteMobi2Buy.Infrastructure.Messaging.Events;
using TesteMobi2Buy.Infrastructure.Messaging.Interfaces;

namespace TesteMobi2Buy.Tests.Services;

public class ClienteAppServiceTests
{
    private readonly Mock<IClienteRepository> _clienteRepositoryMock;
    private readonly Mock<IViaCepService> _viaCepServiceMock;
    private readonly Mock<IClienteCadastradoProducer> _clienteCadastradoProducerMock;
    private readonly ClienteAppService _clienteAppService;

    public ClienteAppServiceTests()
    {
        _clienteRepositoryMock = new Mock<IClienteRepository>();
        _viaCepServiceMock = new Mock<IViaCepService>();
        _clienteCadastradoProducerMock = new Mock<IClienteCadastradoProducer>();

        _clienteAppService = new ClienteAppService(
            _clienteRepositoryMock.Object,
            _viaCepServiceMock.Object,
            _clienteCadastradoProducerMock.Object
        );
    }

    [Fact]
    public async Task CriarClienteAsync_DeveCriarCliente_QuandoDadosValidos()
    {
        var dto = new CreateClienteDto
        {
            Nome = "João",
            Email = "joao@email.com",
            Cep = "01001000"
        };

        _clienteRepositoryMock.Setup(repo => repo.ObterPorEmailAsync(dto.Email))
            .ReturnsAsync((Cliente)null);

        _viaCepServiceMock.Setup(service => service.ConsultarCepAsync(dto.Cep))
            .ReturnsAsync(new ViaCepResponse
            {
                Logradouro = "Rua A",
                Bairro = "Bairro B",
                Localidade = "Cidade C",
                Uf = "SP"
            });

        _clienteRepositoryMock.Setup(repo => repo.AdicionarAsync(It.IsAny<Cliente>()))
            .Returns(Task.CompletedTask);

        var cliente = await _clienteAppService.CriarClienteAsync(dto);

        Assert.Equal(dto.Nome, cliente.Nome);
        Assert.Equal(dto.Email, cliente.Email);
        Assert.Equal(dto.Cep, cliente.Cep);

        _clienteCadastradoProducerMock.Verify(p =>
            p.Publicar("cliente_criado", It.IsAny<ClienteCriadoEvent>()), Times.Once);
    }

    [Fact]
    public async Task AtualizarClienteAsync_DeveAtualizarCliente_QuandoDadosValidos()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var clienteExistente = new Cliente
        {
            Id = clienteId,
            Nome = "Antigo Nome",
            Email = "antigo@email.com",
            Cep = "01001000"
        };

        var dto = new UpdateClienteDto
        {
            Nome = "Novo Nome",
            Email = "novo@email.com",
            Cep = "01001000"
        };

        _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(clienteId))
            .ReturnsAsync(clienteExistente);

        _clienteRepositoryMock.Setup(r => r.ObterPorEmailAsync(dto.Email))
            .ReturnsAsync((Cliente)null);

        _viaCepServiceMock.Setup(s => s.ConsultarCepAsync(dto.Cep))
            .ReturnsAsync(new ViaCepResponse
            {
                Logradouro = "Nova Rua",
                Bairro = "Novo Bairro",
                Localidade = "Nova Cidade",
                Uf = "SP"
            });

        _clienteRepositoryMock.Setup(r => r.AtualizarAsync(It.IsAny<Cliente>()))
            .Returns(Task.CompletedTask);

        await _clienteAppService.AtualizarClienteAsync(clienteId, dto);

        _clienteRepositoryMock.Verify(r => r.AtualizarAsync(It.Is<Cliente>(
            c => c.Nome == dto.Nome && c.Email == dto.Email && c.Cep == dto.Cep
        )), Times.Once);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveRetornarCliente_QuandoEncontrado()
    {
        // Arrange
        var clienteId = Guid.NewGuid();
        var cliente = new Cliente
        {
            Id = clienteId,
            Nome = "Cliente Teste",
            Email = "teste@email.com",
            Cep = "01001000"
        };

        _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(clienteId))
            .ReturnsAsync(cliente);

        var resultado = await _clienteAppService.ObterPorIdAsync(clienteId);

        Assert.Equal(clienteId, resultado.Id);
        Assert.Equal("Cliente Teste", resultado.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_DeveLancarExcecao_QuandoNaoEncontrado()
    {
        var clienteId = Guid.NewGuid();

        _clienteRepositoryMock.Setup(r => r.ObterPorIdAsync(clienteId))
            .ReturnsAsync((Cliente)null);

        var ex = await Assert.ThrowsAsync<ApplicationException>(() =>
            _clienteAppService.ObterPorIdAsync(clienteId));

        Assert.Equal("Cliente não encontrado.", ex.Message);
    }
}
