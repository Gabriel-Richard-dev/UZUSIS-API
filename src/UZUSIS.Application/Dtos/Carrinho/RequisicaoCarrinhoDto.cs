namespace UZUSIS.Application.Dtos.Carrinho;

public class RequisicaoCarrinhoDto
{
    public long ClienteId { get; set; }
    public long ProdutoId { get; set; }
    public string Sigla { get; set; }
    public int Quantidade { get; set; }
}