using Tech.Challenge.OptionsSetup;

namespace Tech.Challenge.Configuration;

public static class Options
{
    public static IServiceCollection ConfigureOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();
        services.ConfigureOptions<MailOptionsSetup>();

        return services;
    }
}
