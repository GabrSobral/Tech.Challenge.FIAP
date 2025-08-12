using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Tech.Challenge.Application.Core;
using Tech.Challenge.OptionsSetup;

namespace Tech.Challenge.Configuration;

public static class Authentication
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        using var sp = services.BuildServiceProvider();
        var jwtOptions = sp.GetRequiredService<IOptions<JwtOptions>>().Value;

        services
            .AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SecretKey)),
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/chat"))
                            context.Token = accessToken;

                        return Task.CompletedTask;
                    }
                };
            });

        services.AddAuthorization();

        return services;
    }
}
