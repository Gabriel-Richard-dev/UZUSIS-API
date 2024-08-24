using UZUSIS.Domain.Entities;

namespace UZUSIS.Domain.Contracts.Repositories;

public interface IPedidoRepository : IBaseRepository<Pedido>
{
    Task<List<Pedido>> ObterPedidosCliente(long clienteId);
}