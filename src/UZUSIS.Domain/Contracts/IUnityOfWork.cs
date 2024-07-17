namespace UZUSIS.Domain.Contracts;

public interface IUnityOfWork
{
    Task<bool> Commit();
}