using UZUSIS.Application.Dtos.Usuario;

namespace UZUSIS.Application.Dtos.Administrador;

public interface IAdminAuthService
{
    Task<string> Login(LoginUsuarioDto admin);
}