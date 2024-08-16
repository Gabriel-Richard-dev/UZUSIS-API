using System.Collections.ObjectModel;
using Microsoft.VisualBasic;
using UZUSIS.Application.Dtos.Foto;
using UZUSIS.Application.Dtos.Tamanho;
using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class ProdutoDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }

    public List<TamanhoDto> Tamanhos { get; set; } = new();
    
    
    public List<string> FotoUrls { get; set; }
    
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}