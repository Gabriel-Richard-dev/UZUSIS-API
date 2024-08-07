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
    public async Task<IActionResult> AdicionarProduto([FromBody] ProdutoDto produtoDto)
    {
        return CustomResponse(_produtoService.Adicionar(produtoDto));
    }

    [AllowAnonymous]
    [HttpGet("")]
    public async Task<IActionResult> ObterProdutos([FromQuery] ECategoriaProduto? categoriaProduto = null)
    {
        return CustomResponse(_produtoService.Obter(categoriaProduto));
    }

    [AllowAnonymous]
    [HttpPut("atualizar")]
    public async Task<IActionResult> Atualizar(int produtoId, AtualizarProdutoDto produtoDto)
    {
       
        return CustomResponse( await _produtoService.Atualizar(produtoId, produtoDto));
    }

    [AllowAnonymous]
    [HttpPatch]
    public async Task<IActionResult> AtualizarParcial(int produtoId, AtualizarProdutoDto produtoDto)
    {
        return CustomResponse(await _produtoService.Atualizar(produtoId, produtoDto));
    }

}