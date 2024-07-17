using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;

namespace UZUSIS.API.Controllers.V1.Administrador;

[AllowAnonymous]
public class AdministradorController : BaseController
{

    private readonly IAdministradorService _administradorService;
    public AdministradorController(INotificator notificator, IAdministradorService administradorService) : base(notificator)
    {
        _administradorService = administradorService;
    }


    [AllowAnonymous]
    [HttpPost]
    [Route("Adicionar")]
    public async Task<IActionResult> Criar(AdicionarUsuarioDto user)
    {
        return CustomResponse(await _administradorService.Criar(user));
    }
    
}