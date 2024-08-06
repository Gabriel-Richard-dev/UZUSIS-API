using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Compra : Entity
{
    
    public Cliente Cliente { get; set; }

    public long ClienteId { get; set; }
    public List<Pedido> Pedidos { get; set; }
    public decimal ValorTotal { get; set; }
}