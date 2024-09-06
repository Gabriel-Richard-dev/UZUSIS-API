namespace UZUSIS.Application.Dtos.Pedido;

public class PedidoCarrinhoDto
{
    public long ProdutoId { get; set; }
    public decimal ValorTotal { get; set; }
    public int Quantidade { get; set; }

}