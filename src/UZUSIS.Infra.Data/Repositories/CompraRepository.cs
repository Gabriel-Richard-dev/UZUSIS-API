using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;
using UZUSIS.Domain.Entities.Acessories;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Infra.Data.Repositories;

public class CompraRepository : BaseRepository<Compra>, ICompraRepository
{
    public CompraRepository(ApplicationContext context) : base(context)
    {
    }
    public async Task<Compra?> Adicionar(Compra compra)
    {
        await Context.Compras.AddAsync(compra);

        foreach (var item in compra.Itens)
        {
            var tamanho = Context.Tamanhos.FirstOrDefault(c => c.Id == item.TamanhoId);
            if (tamanho is not null)
            {
                tamanho.Quantidade -= item.Quantidade;
                Context.Tamanhos.Update(tamanho);
            }
        }
        
        return compra;
    }

    public async Task<List<Compra>> Obter()
    {
        var compras = await Context.Compras.AsNoTracking()
            .Include(c => c.Itens)
            .ToListAsync();

        return compras;
    }

    public async Task<List<Compra>> ObterPeloCliente(long clienteId, bool pesquisarEmAndamento = false)
    {
        var comprasUsuario = Context.Compras.AsNoTracking()
            .Where(c => c.ClienteId == clienteId)
            .Include(c => c.Itens);

        if (pesquisarEmAndamento)
        {
            var comprasEmAndamento = comprasUsuario.Where(c => c.Itens.Any(c => c.FoiRecebico == false));
            return await comprasEmAndamento.ToListAsync();
        }

        return await comprasUsuario.ToListAsync();

    }

    public async Task<List<ItemCompra>> ObterItens()
    {
        return await Context.ItemCompras.AsNoTracking().ToListAsync();
    }



    public async Task<ItemCompra?> EnviarItem(long itemCompraId)
    {
        var item = await Context.ItemCompras.FirstOrDefaultAsync(c => c.Id == itemCompraId);

        if(item is null)
            return null;
        
        item.FoiEnviado = true;
        item.AtualizadoEm = DateTime.Now;
        
        Context.ItemCompras.Update(item);

        return item;

    }
}