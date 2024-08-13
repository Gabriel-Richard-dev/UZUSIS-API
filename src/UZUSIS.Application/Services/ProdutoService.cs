using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
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

    public async Task<ProdutoDto?> Adicionar(AdicionarProdutoDto produtoDto)
    {
        
        var produto = Mapper.Map<Produto>(produtoDto);
        
        
        if (produto is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        var tamanhos = Mapper.Map<List<Tamanho>>(produtoDto.Tamanhos);
        produto.Tamanhos = tamanhos;
        // produto.Fotos.Add(Mapper.Map<List<Foto>>(produtoDto.GetFotos()));

        await _produtoRepository.Adicionar(produto);

        if (await CommitChanges())
        {
            await SalvarFotos(produtoDto.FotoFiles);
            
            return Mapper.Map<ProdutoDto>(produto);
        }

        Notificator.Handle("Não foi possível adicionar o produto");
        return null;

    }

    public async Task<List<ProdutoDto>> Obter(ECategoriaProduto? categoriaProduto = null)
    {
        var produtos = await _produtoRepository.Obter(categoriaProduto);
        
        return Mapper.Map<List<ProdutoDto>>(produtos);

    }

    public async Task<List<byte[]>> ObterFoto(long produtoId)
    {
        var produto = await _produtoRepository.Obter(produtoId);

        if (produto is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        List<string> fotoPaths = new List<string>();
        
        foreach (var path in produto.Fotos)
        {
            fotoPaths.Add("../UZUSIS.Infra.Data/Uploads/FotoProduto/" +path.FotoUrl);
        }

        List<byte[]> fotos = new();

        foreach (var path in fotoPaths)
        {
            var reader = await File.ReadAllBytesAsync(path);
            fotos.Add(reader);
        }

        return fotos;


    }


    public async Task<AtualizarProdutoDto?> Atualizar(int produtoId, AtualizarProdutoDto produtoDto)
    {

        var produto = Mapper.Map<Produto>(produtoDto);
        produto.Id = produtoId;
        
        if (produto is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        await _produtoRepository.Atualizar(produto);

        if (await CommitChanges())
        {
            return produtoDto;
        }
        
        Notificator.Handle("Não foi possivel atualizar produto");
        return null;

    }



    private async Task<bool> CommitChanges() => await _produtoRepository.UnitOfWork.Commit();


    private async Task SalvarFotos(List<IFormFile> fotos)
    {
        foreach (var foto in fotos)
        {
            using (var stream = File.Create("../UZUSIS.Infra.Data/Uploads/FotoProduto/" + foto.FileName))
            {
                await foto.CopyToAsync(stream);
            }
        }        
    }

}