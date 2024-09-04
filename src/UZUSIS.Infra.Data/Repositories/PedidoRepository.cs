using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Infra.Data.Repositories;

public class PedidoRepository : BaseRepository<Pedido>, IPedidoRepository
{
    public PedidoRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<List<Pedido>> ObterPedidosCliente(long clienteId)
    {

        var cliente = Context.Clientes.AsNoTrackingWithIdentityResolution()
            .Where(c => c.Id == clienteId).FirstOrDefault();

        var pedidos =
            Context.Pedidos.AsNoTrackingWithIdentityResolution()
                .Where(c => c.CarrinhoId == cliente.CarrinhoId).ToList();

        foreach (var pedido in pedidos)
        {
            pedido.Produto = Context.Produtos.FirstOrDefault(c => c.Id == pedido.ProdutoId)!;
        }
        
        
        return pedidos;


    }
        
     
    public async Task<List<Pedido>> ObterAtivos(long clienteId, long tamanhoId)
    {
        var pedidos = await Context.Pedidos.Where(c => c.ClienteId == clienteId && c.TamanhoId == tamanhoId).ToListAsync();
        return pedidos;
    }

    
    
}