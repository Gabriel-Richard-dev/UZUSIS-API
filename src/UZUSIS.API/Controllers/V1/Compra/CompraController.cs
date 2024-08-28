using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Notification;

namespace UZUSIS.API.Controllers.V1.Compra;

[ApiController]
[Route("[controller]")]
public class CompraController : BaseController
{
    private readonly ICarrinhoService _carrinhoService;
    private readonly ICompraService _compraService;
    public CompraController(INotificator notificator, ICarrinhoService carrinhoService, ICompraService compraService) : base(notificator)
    {
        _carrinhoService = carrinhoService;
        _compraService = compraService;
    }

    [Authorize(Roles = "Cliente")]
    [HttpPost]
    public async Task<IActionResult> ComprarUnico()
    {
        return CustomResponse();
    }

    [Authorize(Roles = "Cliente")]
    [HttpPost("cliente/carrinho")]
    public async Task<IActionResult> ComprarCarrinho()
    {
        await _compraService.ComprarCarrinho();
        return CustomResponse( );
    }

}