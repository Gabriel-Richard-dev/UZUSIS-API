using UZUSIS.Application.Dtos.Produto;
using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Contracts.Services;

public interface IProdutoService
{
    Task<ProdutoDto?> Adicionar(ProdutoDto produtoDto);
    Task<List<ProdutoDto>> Obter(ECategoriaProduto? categoriaProduto = null);
    Task<AtualizarProdutoDto?> Atualizar(int produtoId, AtualizarProdutoDto produtoDto);
}