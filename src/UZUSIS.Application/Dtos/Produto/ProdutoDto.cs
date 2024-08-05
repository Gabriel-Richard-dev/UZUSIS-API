using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class ProdutoDto
{
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}