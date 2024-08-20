using Microsoft.AspNetCore.Http;
using UZUSIS.Application.Dtos.Foto;
using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class AdicionarProdutoDto
{
   
    
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public List<TamanhoDto> Tamanhos { get; set; } = new();
    public List<IFormFile> FotoFiles { get; set; } = new();
    
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}

public class TamanhoDto()
{
    public string Sigla { get; set; }
    public int Quantidade { get; set; }
}