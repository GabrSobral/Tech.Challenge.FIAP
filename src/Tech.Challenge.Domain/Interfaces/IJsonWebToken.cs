namespace Tech.Challenge.Domain.Interfaces;

public record JsonWebTokenPayload(
    Guid UserId,
    string Email,
    string FirstName,
    string? LastName
 );

public interface IJsonWebToken
{
    public string Sign(JsonWebTokenPayload user);

    public T DecryptToken<T>(string token) where T : new();
}
