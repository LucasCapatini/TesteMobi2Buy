using Microsoft.AspNetCore.Mvc;
using TesteMobi2Buy.Application.DTOs;
using TesteMobi2Buy.Application.Interfaces;
using TesteMobi2Buy.Domain.Exceptions;

namespace TesteMobi2Buy.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteAppService _clienteAppService;

    public ClientesController(IClienteAppService clienteAppService)
    {
        _clienteAppService = clienteAppService;
    }

    [HttpPost]
    public async Task<IActionResult> CriarCliente([FromBody] CreateClienteDto dto)
    {
        try
        {
            var cliente = await _clienteAppService.CriarClienteAsync(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = cliente.Id }, cliente);
        }
        catch (ClienteExistenteException clientException)
        {
            return BadRequest(clientException.Message);
        }
        catch (CepInvalidoException cepEception)
        {
            return BadRequest(cepEception.Message);
        }
        catch
        {
            return StatusCode(500, "Ocorreu um erro inesperado. Tente novamente mais tarde.");
        }
    }


    [HttpGet("{id:guid}")]
    public async Task<IActionResult> ObterPorId(Guid id)
    {
        try
        {
            var cliente = await _clienteAppService.ObterPorIdAsync(id);
            return Ok(cliente);
        }
        catch (ClienteNaoEncontradoException clienteNaoEncontrado)
        {
            return NotFound(clienteNaoEncontrado.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, new { erro = "Erro interno. Tente novamente mais tarde." });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCliente(Guid id, [FromBody] UpdateClienteDto dto)
    {
        try
        {
            await _clienteAppService.AtualizarClienteAsync(id, dto);
            return NoContent();
        }
        catch (ClienteExistenteException clientException)
        {
            return BadRequest(clientException.Message);
        }
        catch (CepInvalidoException cepEception)
        {
            return BadRequest(cepEception.Message);
        }
        catch (ClienteNaoEncontradoException clienteNaoEncontradoException)
        {
            return NotFound(clienteNaoEncontradoException.Message);
        }
        catch (Exception)
        {
            return StatusCode(500, new { erro = "Erro interno. Tente novamente mais tarde." });
        }
    }
}