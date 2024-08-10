using UZUSIS.Core.Enums;
using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Produto : Entity
{
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }


    public int QuantidadeTotal
    {
        get
        {
            int quant = 0;
            foreach (var tamanho in Tamanhos)
            {
                quant += tamanho.Quantidade;
            }

            return quant;
        }
    }


    public string SituacaoProduto
    {


        get
        {

            if (QuantidadeTotal <= 0)
            {
                return EStatusProduto.Indisponivel.ToString();
            }

            return EStatusProduto.Disponivel.ToString();
        }
        
    }
    
    public List<Tamanho> Tamanhos { get; set; }

    
    
    
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
    
    public List<Pedido> Pedidos { get; set; }
    
}