using Microsoft.Extensions.DependencyInjection;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Infra.MailService.Implementations;

namespace Tech.Challenge.Infra.MailService;

public static class DependencyInjection
{
    public static IServiceCollection AddInfraMailService(this IServiceCollection services)
    {
        services.AddScoped<IMailService, MailKitEmailSender>();

        return services;
    }
}