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
}