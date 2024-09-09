using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Infra.Data.Repositories;

public class CarrinhoRepository : BaseRepository<Carrinho>, ICarrinhoRepository
{
    public CarrinhoRepository(ApplicationContext context) : base(context)
    {
    }

    public async  Task<Carrinho?> Obter(long id)
    {
        var carrinho = Context.Carrinhos.FirstOrDefault(c => c.Id == id);
        var pedidos = Context.Pedidos.Where(c => c.CarrinhoId == id).ToList();

        carrinho.Pedidos = pedidos;

        return carrinho;

    }
}