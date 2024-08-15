using UZUSIS.Application.Dtos.Usuario;

namespace UZUSIS.Application.Contracts.Services;

public interface IClienteService
{
    Task<AdicionarUsuarioDto?> AdicionarCliente(AdicionarUsuarioDto usuarioDto);
}