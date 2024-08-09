using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
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
        var produto = 
            (from p in Context.Produtos
            join t in Context.Tamanhos 
                on p.Id equals t.ProdutoId
                select p);
        
        if (categoriaProduto is not null)
        {
            return await produto.Where(c => c.Categoria == categoriaProduto).ToListAsync();
        }

        return await produto.ToListAsync();
    }

}