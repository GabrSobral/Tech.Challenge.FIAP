using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Infra.MailService.Core;

namespace Tech.Challenge.Infra.MailService.Implementations;

public class MailKitEmailSender : IMailService
{
    private readonly EmailSettings _emailSettings;

    public MailKitEmailSender(IOptions<MailOptions> options)
    {
        _emailSettings = new EmailSettings 
        { 
            SmtpUser = options.Value.SmtpUser,
            SenderEmail = options.Value.SenderEmail,
            SenderName = options.Value.SenderName,
            SmtpHost = options.Value.SmtpHost,
            SmtpPass = options.Value.SmtpPass,
            SmtpPort = options.Value.SmtpPort
        };
    }

    public async Task SendEmailAsync(SendMailSettings settings, CancellationToken cancellationToken)
    {
        var email = new MimeMessage();

        email.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.SenderEmail));
        email.To.Add(MailboxAddress.Parse(settings.To));

        email.Subject = settings.Subject;
        email.Body = new TextPart("html") { Text = settings.Body };

        using var client = new SmtpClient();

        await client.ConnectAsync(
             _emailSettings.SmtpHost,
             _emailSettings.SmtpPort,
             _emailSettings.SmtpPort == 465
                 ? SecureSocketOptions.SslOnConnect
                 : SecureSocketOptions.StartTls,
             cancellationToken
         );
        await client.AuthenticateAsync(_emailSettings.SmtpUser, _emailSettings.SmtpPass, cancellationToken);
        await client.SendAsync(email, cancellationToken);
        await client.DisconnectAsync(true, cancellationToken);
    }

    public async Task SendOrdemServicoStatusMail(Email email, EServiceOrderStatus orderStatus, CancellationToken cancellationToken)
    {
        var settings = new SendMailSettings
        {
            To = email.Endereco,
            Body = $"Ordem de Serviço atualizada para o status {orderStatus}",
            Subject = "Statua da Ordem de Serviço"
        };

        await SendEmailAsync(settings, cancellationToken);
    }
}


