namespace UZUSIS.Application.Contracts.Services;

public interface IEmailService
{
    Task EnviarConfirmacao(string email);
}