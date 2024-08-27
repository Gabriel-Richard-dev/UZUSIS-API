using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Endereco;
using UZUSIS.Application.Dtos.Usuario;

namespace UZUSIS.Application.Contracts.Services;

public interface IClienteService
{
    Task<ClienteDto?> ObterCliente();
    Task<ClienteDto?> AdicionarCliente(AdicionarClienteDto usuarioDto);
}