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
        return CustomResponse(await _compraService.ComprarCarrinho());
    }
    
    [Authorize(Roles = "Cliente")]
    [HttpGet("cliente/historico")]
    public async Task<IActionResult> HistoricoCliente()
    {
        return CustomResponse(await _compraService.ObterHistorico());
    } 
    
    [Authorize(Roles = "Cliente")]
    [HttpGet("cliente/em-andamento")]
    public async Task<IActionResult> EmAndamentoCliente()
    {
        return CustomResponse(await _compraService.ObterEmAndamento());
    }
    
    [Authorize(Roles = "Administrador")]
    [HttpGet("administrador/dashboard")]
    public async Task<IActionResult> HistoricoAdministrador()
    {
        return CustomResponse(await _compraService.ObterTodosOsPedidos());
    }
    
    [Authorize(Roles = "Administrador")]
    [HttpPatch("administrador/dashboard/enviar-produto")]
    public async Task<IActionResult> EnviarItem([FromQuery] long itemCompraId)
    {
        return CustomResponse(await _compraService.EnviarItemCompra(itemCompraId));
    }
    
    
    
    

}