using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Endereco;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;

namespace UZUSIS.API.Controllers.V1.Cliente;

[AllowAnonymous]
[Route("[controller]")]
public class ClienteAuth : BaseController
{
    private readonly IClienteAuthService _clienteAuthService;
    private readonly IClienteService _clienteService;
    private readonly IEmailService _emailService;
    
    public ClienteAuth(INotificator notificator, IClienteAuthService clienteAuthService, IEmailService emailService, IClienteService clienteService) : base(notificator)
    {
        _clienteAuthService = clienteAuthService;
        _emailService = emailService;
        _clienteService = clienteService;
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginUsuarioDto dto)
    {
        var token = await _clienteAuthService.Login(dto);
        return token != null ? Ok(token) : Unauthorized(new[] { "Email/senha incorretas." });
    }

    [AllowAnonymous]
    [HttpPost("enviar-confirmacao-email")]
    public async Task<IActionResult> Cadastrar([FromBody]ClienteEnviarEmailConfirmacaoDto email)
    {
        await _emailService.EnviarConfirmacao(email.Email);
        return CustomResponse("Um código de confirmação foi enviado para o email em questão.");
    }
    
    [AllowAnonymous]
    [HttpGet("codigo-valido")]
    public async Task<IActionResult> ValidarCodigo(string email, string codigo)
    {
        return CustomResponse(await _clienteAuthService.CodigoValido(email, codigo));
    }
    
    [AllowAnonymous]
    [HttpPost("cadastrar")]
    public async Task<IActionResult> Adicionar(AdicionarClienteDto usuarioDto)
    {
        return CustomResponse(await _clienteService.AdicionarCliente(usuarioDto));
    }
    

}