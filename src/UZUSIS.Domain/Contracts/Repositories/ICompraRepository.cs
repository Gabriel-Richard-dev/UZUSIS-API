using UZUSIS.Domain.Entities;
using UZUSIS.Domain.Entities.Acessories;

namespace UZUSIS.Domain.Contracts.Repositories;

public interface ICompraRepository : IBaseRepository<Compra>
{
    Task<List<Compra>> ObterPeloCliente(long clienteId, bool pesquisarEmAndamento = false);
    Task<List<ItemCompra>> ObterItens();

    Task<ItemCompra?> EnviarItem(long itemCompraId);
}