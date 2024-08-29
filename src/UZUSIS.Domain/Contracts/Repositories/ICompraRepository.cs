using UZUSIS.Domain.Entities;
using UZUSIS.Domain.Entities.Acessories;

namespace UZUSIS.Domain.Contracts.Repositories;

public interface ICompraRepository : IBaseRepository<Compra>
{
    Task Comprar(CompraPedido compra);
}