using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Categoria;
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
        
        var tamanhos = new List<TamanhoDto>()
        {
            new TamanhoDto()
            { Sigla = "P", Quantidade = produtoDto.QuantidadeP },
            new TamanhoDto()
            { Sigla = "M", Quantidade = produtoDto.QuantidadeM },
            new TamanhoDto()
            { Sigla = "G", Quantidade = produtoDto.QuantidadeG }
        };

        produto.Tamanhos = Mapper.Map<List<Tamanho>>(tamanhos);

        List<Foto> fotosProduto = new();
        foreach (var foto in produtoDto.FotoFiles)
        {
            fotosProduto.Add(new Foto()
            {
                FotoUrl = Guid.NewGuid()
                    .ToString()
                    .Replace("-", string.Empty) + Path.GetExtension(foto.FileName)
            });
        }

        produto.Fotos = fotosProduto;
        Notificator.Handle(produto.Validate());
        
        if(Notificator.HasNotification)
            return null;
        
        
        await _produtoRepository.Adicionar(produto);
        
        if (await CommitChanges())
        {
            var fotoUrls = await SalvarFotos(produtoDto, fotosProduto); 
            var dto = Mapper.Map<ProdutoDto>(produto);
            dto.FotoUrls = fotoUrls;
            return dto;
        }

        Notificator.Handle("Não foi possível adicionar o produto");
        return null;

    }

    public async Task<List<ProdutoDto>> Obter(ECategoriaProduto? categoriaProduto = null)
    {
        var produtos = await _produtoRepository.Obter(categoriaProduto);
        var produtoRetorno =  Mapper.Map<List<ProdutoDto>>(produtos);
        foreach (var produto in produtoRetorno)
        {
            var fotos = await GetUrlsFotos(produto.Id);
            

            for (int i = 0; i < fotos.Urls.Count; i++)
            {
                produto.FotoUrls.Add(fotos.Urls[i]);
            }
        }
        
        return produtoRetorno;
    }

    public async Task<ProdutoDto?> ObterPorId(long produtoId)
    {
        var produto = await _produtoRepository.ObterPorId(produtoId);
    
        if (produto is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
    
        var produtoDto = Mapper.Map<ProdutoDto>(produto);
        
        var fotosUrls = await GetUrlsFotos(produtoId);
       
        foreach (var url in fotosUrls.Urls)
        {
            produtoDto.FotoUrls.Add(url);
        }
        
    
        return produtoDto;
    }

    public async Task<List<ProdutoDto>> ObterNome(string nome)
    {
        var produtos = Mapper.Map<List<ProdutoDto>>(await _produtoRepository.ObterPorNome(nome));

        foreach (var produto in produtos)
        {
            var fotos = await GetUrlsFotos(produto.Id);
            

            for (int i = 0; i < fotos.Urls.Count; i++)
            {
                produto.FotoUrls.Add(fotos.Urls[i]);
            }
        }

        return produtos;
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

    public async Task<List<CategoriaDto>> ObterCategorias()
    {
        var categorias = new List<CategoriaDto>();
        var enumCategoria = Enum.GetValues(typeof(ECategoriaProduto));

        foreach (var c in enumCategoria)
        {
            categorias.Add(new CategoriaDto()
            {
                Categoria = (ECategoriaProduto)c,
                NomeCategoria = c.ToString()!.Equals("Calca") ? "Calça" : c.ToString()!
            });
        }

        return categorias;
    }

    public async Task<List<ProdutoDto>> DashBoardAdmin()
    {
        var produtos = await _produtoRepository.Obter();
        var produtoRetorno =  Mapper.Map<List<ProdutoDto>>(produtos);
        foreach (var produto in produtoRetorno)
        {
            var fotos = await GetUrlsFotos(produto.Id);
            

            for (int i = 0; i < fotos.Urls.Count; i++)
            {
                produto.FotoUrls.Add(fotos.Urls[i]);
            }
        }
        
        return produtoRetorno;
    }


    private async Task<bool> CommitChanges() => await _produtoRepository.UnitOfWork.Commit();


    private async Task<List<string>> SalvarFotos(AdicionarProdutoDto dto, List<Foto> photoName)
    {

        int contador = 0;
        var fotoReturn = new List<string>();
        foreach (var foto in dto.FotoFiles)
        {
            var nome = photoName[contador].FotoUrl;
            fotoReturn.Add(nome);
            using (var stream = File.Create("../UZUSIS.Infra.Data/Uploads/FotoProduto/" +  nome))
            {
                await foto.CopyToAsync(stream);
            }

            contador++;
        }
        return fotoReturn;
    }
    
    private async Task<FotoViewModel> GetUrlsFotos(long id)
    {
        int quantidadeFotos = (await ObterFoto(id)).Count();

        var apiUrl = _httpContextAccessor.HttpContext!.Request.Host;
        
        List<string> apiUrls = new();
        for (int i = 0; i < quantidadeFotos; i ++)
        {
            
            apiUrls.Add("http://" + apiUrl+$"/produto/{id}/foto/{i}");
     
        }
        
        return new FotoViewModel()
        {
            Urls = apiUrls
        };


    }

}