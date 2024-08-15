using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;


namespace UZUSIS.API.Controllers.Cliente;



[ApiController]
[Route("[controller]")]
public class ClienteController : BaseController
{
    private readonly IClienteService _clienteService;
    
    public ClienteController(INotificator notificator, IClienteService clienteService) : base(notificator)
    {
        _clienteService = clienteService;
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