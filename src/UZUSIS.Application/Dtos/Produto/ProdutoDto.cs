using UZUSIS.Application.Dtos.Foto;
using UZUSIS.Application.Dtos.Tamanho;
using UZUSIS.Core.Enums;

namespace UZUSIS.Application.Dtos.Produto;

public class ProdutoDto
{
    public long Id { get; set; }
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public List<TamanhoDto> Tamanhos { get; set; }
    public List<FotoProdutoDto> Fotos { get; set; }

    public List<string> FotoUrls
    {
        get
        {
            var urls = new List<string>();
            foreach (var foto in Fotos)
            {
                urls.Add(foto.FotoUrl);
            }

            return urls;
        }
    }

    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}