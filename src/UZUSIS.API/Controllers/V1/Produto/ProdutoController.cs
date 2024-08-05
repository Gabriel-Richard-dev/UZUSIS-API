using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Produto;
using UZUSIS.Application.Notification;

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
        return CustomResponse(await _produtoService.Adicionar(produtoDto));
    }
    
}