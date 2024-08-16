using Microsoft.AspNetCore.Http;
using UZUSIS.Application.Dtos.Foto;
using UZUSIS.Application.Dtos.Tamanho;
using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class AdicionarProdutoDto
{
    

    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public List<TamanhoDto>? Tamanhos { get; set; }
    public List<IFormFile> FotoFiles { get; set; } 
    
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}