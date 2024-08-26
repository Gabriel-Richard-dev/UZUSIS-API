using AutoMapper;
using Microsoft.AspNetCore.Identity;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Endereco;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class ClienteService : BaseService, IClienteService
{

    private readonly IClienteRepository _clienteRepository;
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IPasswordHasher<Cliente> _hasher;
       
    public ClienteService(INotificator notificator, IMapper mapper, IAdministradorRepository administradorRepository, IClienteRepository clienteRepository, ICarrinhoRepository carrinhoRepository, IPasswordHasher<Cliente> hasher) : base(notificator, mapper)
    {
        _clienteRepository = clienteRepository;
        _carrinhoRepository = carrinhoRepository;
        _hasher = hasher;
    }


    public async Task<bool> ValidarCodigoConfirmacao(string email, string codigoConfirmacao)
    {

        return true;


    }
    
    public async Task<ClienteDto?> AdicionarCliente(AdicionarClienteDto usuarioDto)
    {
        
        var userExists = (await _clienteRepository.Obter(usuarioDto.Email));
        
        if (userExists is not null)
        {
            Notificator.Handle("Usuario com um email cadastrado já existente.");
            return null;
        }
        
        
        var cliente = Mapper.Map<Cliente>(usuarioDto);
        if (cliente is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }

        Endereco endereco = Mapper.Map<Endereco>(usuarioDto.Endereco);

        cliente.Endereco = endereco;
        cliente.Senha = _hasher.HashPassword(cliente, cliente.Senha);
        var clienteDb = await _clienteRepository.Adicionar(cliente);
        
        
        if (await CommitChanges())
        {
            return Mapper.Map<ClienteDto>(usuarioDto);
        }
        
        Notificator.Handle("Não foi possivel criar o usuário");
        return null;
    }

    public async Task<ClienteDto?> ObterCliente(long id)
    {
        return Mapper.Map<ClienteDto>(await _clienteRepository.Obter(id));
    }

    private async Task<bool> CommitChanges() => await _clienteRepository.UnitOfWork.Commit();

}