using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Enums;

namespace Tech.Challenge.Domain.Interfaces;

public interface IMailService
{
    Task SendEmailAsync(SendMailSettings settings, CancellationToken cancellationToken);
    Task SendOrdemServicoStatusMail(Email email, EServiceOrderStatus orderStatus, CancellationToken cancellationToken);
}

public class SendMailSettings
{
    public string To { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
}

public class EmailSettings
{
    public string SmtpHost { get; set; } = null!;
    public int SmtpPort { get; set; }
    public string SmtpUser { get; set; } = null!;
    public string SmtpPass { get; set; } = null!;
    public string SenderName { get; set; } = null!;
    public string SenderEmail { get; set; } = null!;
}
