using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Foto
{
    public long Id { get; set; }
    public byte[]? FotoBytes { get; set; }
    public long ProdutoId { get; set; }


    public Produto Produto { get; set; }
}