using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Carrinho;
using UZUSIS.Application.Notification;

namespace UZUSIS.API.Controllers.V1.Carrinho;
[AllowAnonymous]
public class CarrinhoController : BaseController
{
    private readonly ICarrinhoService _carrinhoService;
    public CarrinhoController(INotificator notificator, ICarrinhoService carrinhoService) : base(notificator)
    {
        _carrinhoService = carrinhoService;
    }
    
    [Authorize(Roles = "Cliente")]
    [HttpPost("Adicionar-ao-Carrinho")]
    public async Task<IActionResult> AdicionarAoCarrinho(RequisicaoCarrinhoDto requisicaoCarrinhoDto)
    {
        return CustomResponse(await _carrinhoService.AdicionarAoCarrinho(requisicaoCarrinhoDto));
    }

    [Authorize(Roles = "Cliente")]
    [HttpGet("pedidos")]
    public async Task<IActionResult> ObterPedidos()
    {
        return CustomResponse(await _carrinhoService.ObterPedidos());
    }
}