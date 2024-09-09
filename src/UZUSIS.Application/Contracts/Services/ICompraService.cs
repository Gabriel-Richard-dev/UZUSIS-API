using UZUSIS.Application.Dtos.Compra;

namespace UZUSIS.Application.Contracts.Services;

public interface ICompraService
{
    Task<bool> ComprarCarrinho();
    Task<List<CompraDto>> ObterHistorico();
    Task<List<CompraDto>> ObterEmAndamento();
    
    Task<List<ItemCompraDto>> ObterTodosOsPedidos();
    
    Task<ItemCompraDto?> EnviarItemCompra(long itemCompraId);
    
}