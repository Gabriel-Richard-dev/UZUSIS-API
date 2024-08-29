namespace UZUSIS.Application.Contracts.Services;

public interface ICompraService
{
    Task<bool> ComprarCarrinho();
}