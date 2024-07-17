using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UZUSIS.Application.Notification;

namespace UZUSIS.API.Controllers.V1.Administrador;

[AllowAnonymous]
public class AdministradorController : BaseController
{
    public AdministradorController(INotificator notificator) : base(notificator)
    {
    }


    [AllowAnonymous]
    [HttpGet]
    [Route("oi")]
    public async Task<IActionResult> Oi()
    {
        return CustomResponse("oi");
    }
    
}