using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Usuario;

namespace UZUSIS.Application.Contracts.Services;

public interface IClienteService
{
    Task<ClienteDto?> AdicionarCliente(AdicionarUsuarioDto usuarioDto);
}