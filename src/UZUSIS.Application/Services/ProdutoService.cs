using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Produto;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Enums;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(INotificator notificator, IMapper mapper, IProdutoRepository produtoRepository) : base(
        notificator, mapper)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task<ProdutoDto?> Adicionar(ProdutoDto produtoDto)
    {

        var produto = Mapper.Map<Produto>(produtoDto);

        if (produto is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        await _produtoRepository.Adicionar(produto);

        if (await CommitChanges())
        {
            return produtoDto;
        }

        Notificator.Handle("Não foi possível adicionar o produto");
        return null;

    }

    public async Task<List<ProdutoDto>> Obter(ECategoriaProduto? categoriaProduto = null)
    {
        var produtos = await _produtoRepository.Obter(categoriaProduto);

        return Mapper.Map<List<ProdutoDto>>(produtos);

    }


    public async Task Atualizar(int produtoId, ProdutoDto produtoDto)
    {

        var produto = await _produtoRepository.Obter(produtoId);
        var produtoAtualizado = Mapper.Map<Produto>(produtoDto);

        if (produto is null)
        {
            Notificator.HandleNotFoundResource();
            return;
        }
        
        produto.Nome = produtoAtualizado.Nome;
        produto.Quantidade = produtoAtualizado.Quantidade;
        produto.Categoria = produtoAtualizado.Categoria;
        produto.Descricao = produtoAtualizado.Descricao;

        await _produtoRepository.Atualizar(produto);

        if (await CommitChanges())
        {
            return;
        }
        
        Notificator.Handle("Não foi possivel atualizar produto");
   

    }



    private async Task<bool> CommitChanges() => await _produtoRepository.UnitOfWork.Commit();


}