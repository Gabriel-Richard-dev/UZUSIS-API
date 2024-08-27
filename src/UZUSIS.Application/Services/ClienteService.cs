using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Endereco;
using UZUSIS.Application.Dtos.Usuario;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Extensions;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class ClienteService : BaseService, IClienteService
{

    private readonly IClienteRepository _clienteRepository;
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IPasswordHasher<Cliente> _hasher;
    private readonly IPasswordHasher<ConfirmacaoEmail> _hasherConfirmacao;
    private readonly IHttpContextAccessor _httpContext;
       
    public ClienteService(INotificator notificator, IMapper mapper, IAdministradorRepository administradorRepository, IClienteRepository clienteRepository, ICarrinhoRepository carrinhoRepository, IPasswordHasher<Cliente> hasher, IHttpContextAccessor httpContext) : base(notificator, mapper)
    {
        _clienteRepository = clienteRepository;
        _carrinhoRepository = carrinhoRepository;
        _hasher = hasher;
        _httpContext = httpContext;
    }


    public async Task<bool> ValidarCodigoConfirmacao(string email, string codigoConfirmacao)
    {
        var confirmacao = await _clienteRepository.ObterPedidoDeConfirmacao(email);

        if (confirmacao is null)
            return false;

        bool confirmacaoValida =
            _hasherConfirmacao.VerifyHashedPassword(confirmacao, confirmacao.Codigo, codigoConfirmacao) !=
            PasswordVerificationResult.Failed;
        
        if (confirmacaoValida)
        {
            confirmacao.Confirmado();
            await _clienteRepository.ConfirmacaoValidada(confirmacao);
            await _clienteRepository.UnitOfWork.Commit();
            return true;
        }
        return false;
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

    public async Task<ClienteDto?> ObterCliente()
    {
        long id = await ObterIdUsuarioAutenticado();

        if (Notificator.HasNotification)
        {
            return null;
        }
        
        var cliente = await _clienteRepository.Obter(id);
        if (cliente == null)
            return null;

        return Mapper.Map<ClienteDto>(cliente);
    }
       
    private async Task<long> ObterIdUsuarioAutenticado()
    {
        if (_httpContext is null)
        {
            Notificator.Handle("Impossivel encontrar o httpContext");
            return 0;
        }

        long? usuarioId = _httpContext.ObterUsuarioId();
        if (usuarioId == null)
        {
            Notificator.HandleNotFoundResource();
            return 0;
        }

        long id = usuarioId.Value;

        return id;
    }
    private async Task<bool> CommitChanges() => await _clienteRepository.UnitOfWork.Commit();

}



