using AutoMapper;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Dtos.Cliente;
using UZUSIS.Application.Notification;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class CarrinhoService : BaseService, ICarrinhoService
{
    private readonly ICarrinhoRepository _carrinhoRepository;   
    private readonly IClienteRepository _clienteRepository;   
    
    
    public CarrinhoService(INotificator notificator, IMapper mapper, ICarrinhoRepository carrinhoRepository, IClienteRepository clienteRepository) 
        : base(notificator, mapper)
    {
        _carrinhoRepository = carrinhoRepository;
        _clienteRepository = clienteRepository;
    }


    public async Task Adicionar(ClienteDto dto)
    {
        var cliente = await _clienteRepository.Obter(dto.Email);


        if (await _carrinhoRepository.UnitOfWork.Commit())
            return;
        
        Notificator.Handle("Carrinho não adicionado");
        return;

    }
    
    
    
    
    
}