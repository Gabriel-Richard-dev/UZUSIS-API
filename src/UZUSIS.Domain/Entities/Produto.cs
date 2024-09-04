using UZUSIS.Core.Enums;
using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Produto : Entity
{
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public List<Tamanho> Tamanhos { get; set; } = new();
    public List<Foto> Fotos { get; set; }

    List<string> FotoUrls { get; set; } = new();

    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
    
    public List<Pedido> Pedidos { get; set; }
    
}