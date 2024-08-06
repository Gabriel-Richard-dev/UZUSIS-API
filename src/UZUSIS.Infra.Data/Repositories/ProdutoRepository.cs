using Microsoft.EntityFrameworkCore;
using UZUSIS.Core.Enums;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;
using UZUSIS.Infra.Data.Context;

namespace UZUSIS.Infra.Data.Repositories;

public class ProdutoRepository : BaseRepository<Produto>, IProdutoRepository
{
    public ProdutoRepository(ApplicationContext context) : base(context)
    {
    }

    public async Task<List<Produto>> Obter(ECategoriaProduto? categoriaProduto = null)
    {
        if (categoriaProduto is not null)
        {
            return await Context.Produtos.Where(c => c.Categoria == categoriaProduto).ToListAsync();
        }

        return await Context.Produtos.ToListAsync();
    }

}