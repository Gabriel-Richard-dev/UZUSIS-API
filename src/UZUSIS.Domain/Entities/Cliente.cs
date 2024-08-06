using UZUSIS.Core.Enums;
using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Cliente : Usuario
{
    public Cliente()
    {
        TipoUsuario = ETipoUsuario.Cliente;
    }

    public long CarrinhoId { get; set; }

    public Carrinho Carrinho { get; set; }
    public List<Compra> Compras { get; set; }
    public List<Pedido> Pedidos { get; set;  }


}