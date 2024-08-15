using AutoMapper;
using UZUSIS.Application.Contracts.Services;
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
    
    public async Task<AdicionarUsuarioDto?> AdicionarCliente(AdicionarUsuarioDto usuarioDto)
    {

        var userExists = (await _clienteRepository.Obter(usuarioDto.Email));

        if (userExists is not null)
        {
            Notificator.Handle("Usuario com um email cadastrado já existente.");
            return null;
        }
        
        
        var usuario = Mapper.Map<Cliente>(usuarioDto);

        if (usuario is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }
        
        
        await _clienteRepository.Adicionar(usuario);

        if (await CommitChanges())
        {
            return usuarioDto;
        }
        
        Notificator.Handle("Não foi possivel criar o usuário");
        return null;
    }



    private async Task<bool> CommitChanges() => await _clienteRepository.UnitOfWork.Commit();

}