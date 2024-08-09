using System;
using System.Collections.Generic;
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
    public long EnderecoId { get; set; }
    public Endereco Endereco { get; set; }
    public string CPF { get; set; }
    public string Celular { get; set; }
    public DateTime DataNascimento { get; set; }

}