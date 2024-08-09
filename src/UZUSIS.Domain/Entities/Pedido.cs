using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Pedido : Entity
{
    public Produto Produto { get; set; }
    public Cliente Cliente { get; set; }
    public Compra Compra { get; set; }
    public Carrinho Carrinho { get; set; }

    public long CarrinhoId { get; set; }
    public long ProdutoId { get; set; }
    public long ClienteId { get; set; }
    public long CompraId { get; set; }


    public int Quantidade { get; set; }
    public decimal ValorPedido { get; set; }
    
}