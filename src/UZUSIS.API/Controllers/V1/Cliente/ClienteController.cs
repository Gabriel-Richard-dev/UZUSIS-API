using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Notification;

namespace UZUSIS.API.Controllers.V1.Cliente;



[ApiController]
[Route("[controller]")]
public class ClienteController : BaseController
{
    private readonly IClienteService _clienteService;
    private readonly ICarrinhoService _carrinhoService;
    
    public ClienteController(INotificator notificator, IClienteService clienteService, ICarrinhoService carrinhoService) : base(notificator)
    {
        _clienteService = clienteService;
        _carrinhoService = carrinhoService;
    }
    
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Obter([FromQuery]long id)
    {
        return CustomResponse(await _clienteService.ObterCliente(id));
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> Atualizar()
    {
        return CustomResponse();
    }
    
}