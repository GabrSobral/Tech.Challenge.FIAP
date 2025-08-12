using Microsoft.Extensions.Options;
using Tech.Challenge.Application.Core;

namespace Tech.Challenge.OptionsSetup;

public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtOptions";
    private readonly IConfiguration _configuration;

    public JwtOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(JwtOptions options)
    {
        var section = _configuration.GetSection(SectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{SectionName}' is missing.");
        }

        section.Bind(options);

        // Validate required fields
        if (string.IsNullOrWhiteSpace(options.Issuer))
            throw new InvalidOperationException("JwtOptions.Issuer is required and cannot be empty.");

        if (string.IsNullOrWhiteSpace(options.Audience))
            throw new InvalidOperationException("JwtOptions.Audience is required and cannot be empty.");

        if (string.IsNullOrWhiteSpace(options.SecretKey))
            throw new InvalidOperationException("JwtOptions.SecretKey is required and cannot be empty.");

        if (options.ExpireMinutes <= 0)
            throw new InvalidOperationException("JwtOptions.ExpireMinutes must be greater than zero.");
    }
}

