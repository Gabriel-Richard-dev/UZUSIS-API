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
    private readonly ICarrinhoRepository _carrinhoRepository;
       
    public ClienteService(INotificator notificator, IMapper mapper, IAdministradorRepository administradorRepository, IClienteRepository clienteRepository, ICarrinhoRepository carrinhoRepository) : base(notificator, mapper)
    {
        _clienteRepository = clienteRepository;
        _carrinhoRepository = carrinhoRepository;
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

        // cliente.Carrinho = new Carrinho();
        // var clienteBd = await _clienteRepository.Adicionar(cliente);

        // if (await CommitChanges())
        // {
        //     cliente.Carrinho = new Carrinho { ClienteId = clienteBd.Id };
        // }
        //
        // await _ca.Atualizar(clienteBd);
        //

        var clienteDb = await _clienteRepository.Adicionar(cliente);
        // await CommitChanges();
        //
        // clienteDb.Carrinho = new Carrinho { ClienteId = cliente.Id };
        //
        // await _clienteRepository.Atualizar(clienteDb);
        // var carrinho = await _carrinhoRepository.ObterPorId(clienteDb.CarrinhoId);
        //
        // carrinho.ClienteId = clienteDb.Id;
        //
        // await _carrinhoRepository.Atualizar(carrinho);
        
        if (await CommitChanges())
        {
            return Mapper.Map<ClienteDto>(usuarioDto);
        }
        
        Notificator.Handle("Não foi possivel criar o usuário");
        return null;
    }



    private async Task<bool> CommitChanges() => await _clienteRepository.UnitOfWork.Commit();

}