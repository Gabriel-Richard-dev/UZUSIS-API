using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class AtualizarProdutoDto
{
    public string? Nome { get; set; } = null;
    public decimal? Preco { get; set; } = null;
    public int? Quantidade { get; set; } = null;
    public ECategoriaProduto? Categoria { get; set; } = null;
    public string? Descricao { get; set; } = null;
}