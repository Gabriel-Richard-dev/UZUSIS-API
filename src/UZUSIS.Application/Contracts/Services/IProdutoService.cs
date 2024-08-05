using UZUSIS.Application.Dtos.Produto;

namespace UZUSIS.Application.Contracts.Services;

public interface IProdutoService
{
    Task<ProdutoDto?> Adicionar(ProdutoDto produtoDto);
}