using AutoMapper;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class ClienteService : BaseService, IClienteService
{

    private readonly IClienteRepository _clienteRepository;
       
    public ClienteService(INotificator notificator, IMapper mapper, IAdministradorRepository administradorRepository, IClienteRepository clienteRepository) : base(notificator, mapper)
    {
        _clienteRepository = clienteRepository;
    }
    
    public async Task<ClienteDto?> AdicionarCliente(AdicionarUsuarioDto usuarioDto)
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


       
        
        await _clienteRepository.Adicionar(cliente);

        if (await CommitChanges())
        {
            return Mapper.Map<ClienteDto>(usuarioDto);
        }
        
        Notificator.Handle("Não foi possivel criar o usuário");
        return null;
    }



    private async Task<bool> CommitChanges() => await _clienteRepository.UnitOfWork.Commit();

}