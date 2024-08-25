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
    [HttpPost]
    public async Task<IActionResult> Adicionar(AdicionarClienteDto usuarioDto)
    {
        return CustomResponse(await _clienteService.AdicionarCliente(usuarioDto));
    }
    
    [AllowAnonymous]
    [HttpGet]
    public async Task<IActionResult> Obter()
    {
        return CustomResponse();
    }

    [AllowAnonymous]
    [HttpPut]
    public async Task<IActionResult> Atualizar()
    {
        return CustomResponse();
    }
    
}