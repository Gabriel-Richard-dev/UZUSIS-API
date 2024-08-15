using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Infra.Data.Repositories;

public class ClienteRepository : BaseRepository<Cliente>, IClienteRepository
{
    public ClienteRepository(ApplicationContext context) : base(context)
    {
    }
    
    
    public async Task<Cliente?> Obter(string email)
    {
        var cliente = await 
            Context.Clientes
                .FirstOrDefaultAsync(c => c.Email.Equals(email));
        
        return cliente;
    }
}