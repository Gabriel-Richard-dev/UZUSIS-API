using AutoMapper;
using Microsoft.AspNetCore.Http;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Notification;
using UZUSIS.Core.Extensions;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class CompraService : BaseService, ICompraService
{
    
    private readonly ICompraRepository _compraRepository;
    private readonly IClienteRepository _clienteRepository;
    private readonly ICarrinhoRepository _carrinhoRepository;
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public CompraService(INotificator notificator, IMapper mapper, ICompraRepository compraRepository, IHttpContextAccessor httpContextAccessor, IClienteRepository clienteRepository, ICarrinhoRepository carrinhoRepository, IPedidoRepository pedidoRepository) : base(notificator, mapper)
    {
        _compraRepository = compraRepository;
        _httpContextAccessor = httpContextAccessor;
        _clienteRepository = clienteRepository;
        _carrinhoRepository = carrinhoRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task ComprarCarrinho()
    {
        var usuarioId = (int)_httpContextAccessor.ObterUsuarioId()!;
        
        var cliente = await _clienteRepository.Obter(usuarioId);

        if (cliente is null)
        {
            Notificator.Handle("message");
            return;
        }
        
        var carrinho = await _carrinhoRepository.Obter(cliente.CarrinhoId);
        var pedidos = await _pedidoRepository.ObterPedidosCliente(usuarioId);

        Compra compra = new Compra();
        compra.Pedidos = pedidos;
        compra.Cliente = cliente;
        
        await _compraRepository.Adicionar(compra);
        if (await _compraRepository.UnitOfWork.Commit())
        {
            carrinho.FlushCarrinho();
            await _carrinhoRepository.Atualizar(carrinho);
            await _carrinhoRepository.UnitOfWork.Commit();
            return;
        }
        
        Notificator.Handle("Não foi possivel terminar a compra");

    }
    
}