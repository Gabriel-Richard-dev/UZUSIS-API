namespace UZUSIS.Application.Contracts.Services;

public interface IEmailService
{
    Task EnviarConfirmacao(string email);
    Task EnviarRecuperacao(string email);
}