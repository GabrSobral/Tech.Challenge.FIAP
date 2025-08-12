using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Tech.Challenge.Domain.Interfaces;

namespace Tech.Challenge.Application.Core;

/// <summary>
/// Implementation of IJsonWebToken contract
/// </summary>
/// <remarks>
/// JWT constructor responsible for injection dependencies to implementation
/// </remarks>
/// <param name="logger">Logger of class</param>
/// <param name="options">Jwt Options, injected by configuration using IOptions, and abstracted to a class</param>
public class JsonWebToken : IJsonWebToken
{
    private readonly JwtOptions _jwtOptions;
    private readonly ILogger<JsonWebToken> logger;

    public JsonWebToken(ILogger<JsonWebToken> logger, IOptions<JwtOptions> options)
    {
        _jwtOptions = options.Value;
        this.logger = logger ?? throw new ArgumentNullException(nameof(logger), "Logger cannot be null");
    }


    /// <summary>
    /// Method that generate a valid Json Web Token, using user data and HS256
    /// </summary>
    /// <param name="user">User Domain instance</param>
    /// <returns>Token string</returns>
    public string Sign(JsonWebTokenPayload user)
    {
        logger.LogInformation($"Creating a new token to {user.Email}");

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var claims = new Claim[] {
            new (JwtRegisteredClaimNames.Sub, user.UserId.ToString()),
            new (JwtRegisteredClaimNames.Name, user.FirstName),
            new (JwtRegisteredClaimNames.FamilyName, user.LastName ?? ""),
            new (JwtRegisteredClaimNames.Email, user.Email),
        };

        var token = new JwtSecurityToken(
            issuer: _jwtOptions.Issuer,
            audience: _jwtOptions.Audience,
            claims: claims,
            null,
            expires: DateTime.UtcNow.AddSeconds(_jwtOptions.ExpireMinutes),
            signingCredentials: signingCredentials
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

        return tokenString;
    }

    public T DecryptToken<T>(string token) where T : new()
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = _jwtOptions.Issuer,
            ValidAudience = _jwtOptions.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey))
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out var securityToken);
            JwtSecurityToken? jwtToken = securityToken as JwtSecurityToken ?? throw new SecurityTokenException("Invalid token");

            var target = new T();

            foreach (var claim in principal.Claims)
            {
                var property = typeof(T).GetProperty(claim.Type);
                if (property != null && property.CanWrite)
                {
                    var value = Convert.ChangeType(claim.Value, property.PropertyType);
                    property.SetValue(target, value);
                }
            }

            return target;
        }
        catch (Exception ex)
        {
            logger.LogError($"Token validation failed: {ex.Message}");
            throw;
        }
    }
}
