using UZUSIS.Domain.Entities;

namespace UZUSIS.Domain.Contracts.Repositories;

public interface IClienteRepository : IBaseRepository<Cliente>
{
    Task<Cliente?> Obter(string email);
}