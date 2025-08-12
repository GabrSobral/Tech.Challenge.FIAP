using Microsoft.Extensions.DependencyInjection;

namespace Tech.Challenge.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();

        services.AddControllers();

        return services;
    }
}
