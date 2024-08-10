using UZUSIS.Application.Dtos.Tamanho;
using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class ProdutoDto
{
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public int QuantidadeTotal { get; set; }
    public string SituacaoProduto { get; set; }
    public List<TamanhoDto> Tamanhos { get; set; }
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}