using UZUSIS.Application.Dtos.Cliente;

namespace UZUSIS.Application.Contracts.Services;

public interface ICarrinhoService
{
    Task Adicionar(ClienteDto dto);
}