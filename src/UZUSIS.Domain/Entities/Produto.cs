using UZUSIS.Core.Enums;
using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Entities;

public class Produto : Entity
{
    public string Nome { get; set; } = null!;
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public ECategoriaProduto Categoria { get; set; }
    public string Descricao { get; set; } = null!;
}