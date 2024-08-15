using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic.FileIO;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Produto;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Enums;
using UZUSIS.Core.ViewModel;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public ProdutoService(INotificator notificator, IMapper mapper, IProdutoRepository produtoRepository, IHttpContextAccessor httpContextAccessor) : base(
        notificator, mapper)
    {
        _produtoRepository = produtoRepository;
        _httpContextAccessor = httpContextAccessor;
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

        foreach (var produto in produtos)
        {
            var fotos = await GetUrlsFotos((int)produto.Id);
            produto.Fotos.Clear();

            for (int i = 0; i < fotos.Urls.Count; i++)
            {
                
                produto.Fotos.Add(new Foto()
                {
                    FotoUrl = fotos.Urls[i]
                });
            }
        }
        
        
        return Mapper.Map<List<ProdutoDto>>(produtos);

    }

    public async Task<List<byte[]?>> ObterFoto(long produtoId)
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
            fotoPaths.Add("../UZUSIS.Infra.Data/Uploads/FotoProduto/" + path.FotoUrl);
        }

        List<byte[]?> fotos = new();

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


    private async Task SalvarFotos(Produto prod, List<IFormFile> fotos)
    {
        prod.Fotos.Clear();
        var path = "../UZUSIS.Infra.Data/Uploads/FotoProduto/" + 
                   Guid.NewGuid().ToString().Replace("-", string.Empty);
        foreach (var foto in fotos)
        {
            using (var stream = File.Create(path + Path.GetExtension(foto.FileName))) 
            {
                prod.Fotos.Add(path + Path.GetExtension(foto.FileName));
                   await foto.CopyToAsync(stream);
            }
        }        
    }
    
    private async Task<FotoViewModel> GetUrlsFotos(int id)
    {
        int quantidadeFotos = (await ObterFoto(id)).Count();

        var apiUrl = _httpContextAccessor.HttpContext.Request.GetDisplayUrl();
        
        List<string> apiUrls = new();
        for (int i = 0; i < quantidadeFotos; i ++)
        {
            
            apiUrls.Add(apiUrl+$"/{id}/foto/{i}");
     
        }
        
        return new FotoViewModel()
        {
            Urls = apiUrls
        };


    }

}