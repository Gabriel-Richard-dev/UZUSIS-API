using Microsoft.EntityFrameworkCore;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Infra.Data.Repositories;

public class CompraRepository : BaseRepository<Compra>, ICompraRepository
{
    public CompraRepository(ApplicationContext context) : base(context)
    {
    }
}