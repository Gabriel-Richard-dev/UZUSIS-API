using UZUSIS.Domain.Abstractions;

namespace UZUSIS.Domain.Contracts.Repositories;

public interface IBaseRepository<T> where T : Entity
{
    Task<T> Adicionar(T entity);
    Task Atualizar(T entity);
    Task<List<T>> Obter();
}