using Microsoft.EntityFrameworkCore;
using TesteMobi2Buy.Domain.Entities;
using TesteMobi2Buy.Domain.Interfaces;

namespace TesteMobi2Buy.Infrastructure.Data.Repositories;

public class ClienteRepository : IClienteRepository
{
    private readonly AppDbContext _context;

    public ClienteRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AdicionarAsync(Cliente cliente)
    {
        await _context.Clientes.AddAsync(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Cliente cliente)
    {
        _context.Clientes.Update(cliente);
        await _context.SaveChangesAsync();
    }

    public async Task<Cliente?> ObterPorEmailAsync(string email)
    {
        return await _context.Clientes.FirstOrDefaultAsync(c => c.Email == email);
    }

    public async Task<Cliente?> ObterPorIdAsync(Guid id)
    {
        return await _context.Clientes.FindAsync(id);
    }
}
