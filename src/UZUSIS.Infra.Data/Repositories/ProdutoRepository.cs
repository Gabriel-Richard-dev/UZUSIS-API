using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.VisualBasic.CompilerServices;
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
        var query = (from p in Context.Produtos
            join t in Context.Tamanhos on p.Id equals t.ProdutoId into tamanhos
            select new
            {
                Produto = p,
                Tamanhos = tamanhos
            });

       
        foreach (var q in query)
        {
            var tamanhos = q.Tamanhos.Where(c => c.ProdutoId == q.Produto.Id).OrderByDescending(c => c.Sigla);
            if (tamanhos is not null)
            {
                q.Produto.Tamanhos = tamanhos.ToList();
            }
        }

        var produto = await query.Select(c => c.Produto).ToListAsync();
        if (categoriaProduto is not null)
            return produto.Where(c => c.Categoria == categoriaProduto).ToList();

        return produto;

    }

}