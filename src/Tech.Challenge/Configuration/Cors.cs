namespace Tech.Challenge.Configuration;

public static class Cors
{
    public const string POLICY_NAME = "DefaultPolicy";

    public static IServiceCollection ConfigureCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                    //.AllowCredentials()
            });
        });

        return services;
    }
}
