using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Carrinho : Entity
{
    public long ClienteId { get; set; }
    public Cliente Cliente { get; set; }
    public List<Pedido> Pedidos { get; set; }

 
    public void FlushCarrinho()
    {
        Pedidos.Clear();
    }

}