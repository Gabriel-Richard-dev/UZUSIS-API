using System.Collections.Immutable;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
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

        List<Produto> produtos;
        var prods = Context.Produtos;

        
        if (categoriaProduto is not null)
            produtos = prods.Where(c => c.Categoria == categoriaProduto).ToList();
        else
            produtos = prods.ToList();
        
        
     
        foreach (var produto in produtos)
        {
            var tamanhos = Context.Tamanhos.Where(c 
                => c.ProdutoId == produto.Id)
                .OrderByDescending(c => c.Sigla);
            
            produto.Tamanhos = tamanhos.ToList();
            
            var fotos = Context.Fotos.Where(c => c.ProdutoId == produto.Id);
            produto.Fotos = fotos.ToList();

        }
        
        
        
        
        return produtos;

    }


    public async Task<Produto> Obter(long id)
    {
        var produto  = await Context.Produtos.FirstOrDefaultAsync(c=> c.Id == id);

        produto.Tamanhos = Context.Tamanhos.Where(c => c.ProdutoId == produto.Id).ToList();
        produto.Fotos = Context.Fotos.Where(c => c.ProdutoId == produto.Id).ToList();

        return produto;

    }
}