using Microsoft.Extensions.Options;
using Tech.Challenge.Infra.MailService.Core;

namespace Tech.Challenge.OptionsSetup;

public class MailOptionsSetup : IConfigureOptions<MailOptions>
{
    private const string SectionName = "MailOptions";
    private readonly IConfiguration _configuration;

    public MailOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(MailOptions options)
    {
        var section = _configuration.GetSection(SectionName);

        if (!section.Exists())
        {
            throw new InvalidOperationException($"Configuration section '{SectionName}' is missing.");
        }

        section.Bind(options);
    }
}
