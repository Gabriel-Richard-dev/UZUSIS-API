using System.Net;
using System.Net.Mail;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging.Abstractions;
using UZUSIS.Application.Contracts.Services;
using UZUSIS.Application.Notification;
using UZUSIS.Core.EmailAssets;
using UZUSIS.Domain.Contracts.Repositories;
using UZUSIS.Domain.Entities;

namespace UZUSIS.Application.Services;

public class EmailService : BaseService, IEmailService
{

    private readonly IClienteRepository _clienteRepository; 
    private readonly IPasswordHasher<ConfirmacaoEmail> _hasher;
    private readonly IConfiguration _configuration;
    
    public EmailService(INotificator notificator, IMapper mapper, IClienteRepository clienteRepository, IPasswordHasher<ConfirmacaoEmail> hasher, IConfiguration configuration) : base(notificator, mapper)
    {
        _clienteRepository = clienteRepository;
        _hasher = hasher;
        _configuration = configuration;
    }

    public async Task EnviarConfirmacao(string email)
    {
        var pedido = await _clienteRepository.ObterPedidoDeConfirmacao(email);

        if (pedido is not null)
        {
            Notificator.Handle("Um código de confirmação já foi gerado para esse email.");
            return;
        }

        var code = Guid.NewGuid().ToString().Replace("-", "").Substring(0, 5);

        var confirmacaoDto = new ConfirmacaoEmail()
        {
            Codigo = code,
            Email = email,
            Expiracao = DateTime.Now.AddMinutes(2)
        };

        var confirmacao = Mapper.Map<ConfirmacaoEmail>(confirmacaoDto);
        confirmacao.Codigo = _hasher.HashPassword(confirmacao, confirmacao.Codigo);

        await _clienteRepository.GerarConfirmacaoEmail(confirmacao);
        if (await _clienteRepository.UnitOfWork.Commit())
        {

//             var body =
//                 $"""
//                  <h1>Confirme seu email com o codigo abaixo</h1>
//
//                  <br>
//
//                  <h1>{code}</h1>
//
//                  <br>
//
//                  <h6>Compre com qualidade, sys.</h6>
//                  """;
            var body = "teste";




            var mail = new MailData
            {
                EmailSubject = "Confirme seu email agora mesmo!",
                EmailBody = body,
                EmailToId = email
            };

            await SendEmailAsync(mail);
            return;
        }


        Notificator.Handle("Não foi possivel gerar o codigo de confirmação.");
      
    }
    public async Task SendEmailAsync(MailData mailData)
    {
        var emailConfiguration = _configuration.GetSection("EmailConfiguration");
        
        
        var toEmail = mailData.EmailToId;
        var user = emailConfiguration["User"]!;
        var password = emailConfiguration["Password"]!;

        var smtpClient = new SmtpClient(emailConfiguration["Server"])
        {
            Port = int.Parse(emailConfiguration["Port"]!),
            Credentials = new NetworkCredential(user, password),
            EnableSsl = true,
        };

        var mailMessage = new MailMessage(user, toEmail)
        {
            Subject = mailData.EmailSubject,
            Body = mailData.EmailBody,
            IsBodyHtml = true
        };

        try
        {
            await Task.Run(() => smtpClient.Send(mailMessage));
        }
        catch (Exception)
        {
            var notificator = new Notificator();
            notificator.Handle("Ocorreu um erro ao enviar o e-mail");
        }
    }
}