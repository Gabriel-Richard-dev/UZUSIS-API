using System.Drawing;
using System.Net.Mime;
using System.Drawing.Drawing2D;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Produto;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Enums;

namespace UZUSIS.API.Controllers.V1.Produto;

[AllowAnonymous]
public class ProdutoController : BaseController
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(INotificator notificator, IProdutoService produtoService) : base(notificator)
    {
        _produtoService = produtoService;
    }

    [AllowAnonymous]
    [HttpPost("adicionar")]
    public async Task<IActionResult> AdicionarProduto([FromForm] AdicionarProdutoDto produtoDto)
    {
        return CustomResponse(await _produtoService.Adicionar(produtoDto));
    }

    [AllowAnonymous]
    [HttpGet("")]
    public async Task<IActionResult> ObterProdutos([FromQuery] ECategoriaProduto? categoriaProduto = null)
    {
        return CustomResponse(await _produtoService.Obter(categoriaProduto));
    }

    [AllowAnonymous]
    [HttpPut("atualizar")]
    public async Task<IActionResult> Atualizar(int produtoId, AtualizarProdutoDto produtoDto)
    {
       
        return CustomResponse( await _produtoService.Atualizar(produtoId, produtoDto));
    }

    [AllowAnonymous]
    [HttpGet("foto/{id}")]
    public async Task<IActionResult> ObterFotos(long id)
    {
        var fotoBytes = await _produtoService.ObterFoto(id);
        List<System.Drawing.Image> Images = new List<Image>();
        
        
        return CustomResponse(Images);
    }
    
    
    

    [AllowAnonymous]
    [HttpPatch]
    public async Task<IActionResult> AtualizarParcial(int produtoId, AtualizarProdutoDto produtoDto)
    {
        return CustomResponse(await _produtoService.Atualizar(produtoId, produtoDto));
    }

    

}