namespace UZUSIS.Application.Dtos.Produto;

public class RetornoPaginadoDto
{
    public int PaginaAtual { get; set; }
    public int QuantidadePaginas { get; set; }
    public IEnumerable<ProdutoDto> Produtos { get; set; }

}