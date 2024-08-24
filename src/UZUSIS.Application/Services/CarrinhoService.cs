using AutoMapper;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Carrinho;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Dtos.Pedido;
using UZUSIS.Application.Notification;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class CarrinhoService : BaseService, ICarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;   
    private readonly IClienteRepository _clienteRepository;   
    private readonly IProdutoRepository _produtoRepository;   
    private readonly IPedidoRepository _pedidoRepository;   
    
    
    public CarrinhoService(INotificator notificator, IMapper mapper, ICarrinhoRepository carrinhoRepository, IClienteRepository clienteRepository, IProdutoRepository produtoRepository, IPedidoRepository pedidoRepository) 
        : base(notificator, mapper)
    {
        _carrinhoRepository = carrinhoRepository;
        _clienteRepository = clienteRepository;
        _produtoRepository = produtoRepository;
        _pedidoRepository = pedidoRepository;
    }

    public async Task<PedidoDto?> AdicionarAoCarrinho(RequisicaoCarrinhoDto requisicao)
    {
        var cliente = await _clienteRepository.Obter(requisicao.ClienteId);
        var produto = await _produtoRepository.Obter(requisicao.ProdutoId);

        if (cliente is null || produto is null)
        {
            Notificator.HandleNotFoundResource();
            return null;
        }


        var carrinho = await _carrinhoRepository.Obter(cliente.CarrinhoId);

        bool tamanhoForaDoPadrao = !(requisicao.Sigla.ToUpper().Equals("P") || requisicao.Sigla.ToUpper().Equals("M") ||
                                     requisicao.Sigla.ToUpper().Equals("G"));

        if (tamanhoForaDoPadrao)
        {
            Notificator.Handle("Sigla fora do padrão");
            return null;
        }

        var tamanho = produto.Tamanhos.FirstOrDefault(c => c.Sigla.ToUpper().Equals(requisicao.Sigla.ToUpper()));

        if (tamanho.Quantidade < requisicao.Quantidade)
        {
            Notificator.Handle("Pedido excede a quantidade total de produtos");
            return null;
        }

    var pedido = new PedidoDto
        {
            ClienteId = cliente.Id,
            CarrinhoId = carrinho!.Id,
            Quantidade = requisicao.Quantidade,
            ProdutoId = produto.Id,
            TamanhoId = tamanho.Id
        };

    await _pedidoRepository.Adicionar(Mapper.Map<Pedido>(pedido));
    await _pedidoRepository.UnitOfWork.Commit();

    return pedido;

    }

    public async Task<List<PedidoDto>> ObterPedidos(int clienteId)
    {
        var pedidos = await _pedidoRepository.ObterPedidosCliente(clienteId);
        return Mapper.Map<List<PedidoDto>>(pedidos);
    }
}