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

    public async Task<ConfirmacaoEmail?> ObterPedidoDeConfirmacao(string email)
    {
        var confirmacao = await Context.ConfirmacoesDeEmails.AsNoTracking()
            .Where(c => c.FoiConfirmado == false)
            .Where(c => c.Expiracao <= DateTime.Now)
            .FirstOrDefaultAsync(c => c.Email.Equals(email));

        return confirmacao;
    }


    public async Task GerarConfirmacaoEmail(ConfirmacaoEmail confirmacaoEmail)
    {
        Context.ConfirmacoesDeEmails.Add(confirmacaoEmail);
    }
    
    
    
}