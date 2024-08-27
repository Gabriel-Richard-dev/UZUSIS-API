using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Extensions;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Enums;

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
    
    
    [Authorize(Roles = nameof(ETipoUsuario.Cliente))]
    [HttpGet]
    public async Task<IActionResult> Obter()
    {
        return CustomResponse(await _clienteService.ObterCliente());
    }
    

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> Atualizar()
    {
        return CustomResponse();
    }
    
}