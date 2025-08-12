using System.Text.RegularExpressions;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;

namespace Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

public sealed class Email : ValueObject<Email>
{
    private static readonly Regex EmailRegex = new Regex(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Endereco { get; }

    private Email(string endereco)
    {
        Endereco = endereco;
    }

    public static Result<Email> Criar(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return Result.Failure<Email>(new DomainError("E-mail não pode ser vazio."));

        var cleanEmail = email.Trim();

        if (!EmailRegex.IsMatch(cleanEmail))
            return Result.Failure<Email>(new DomainError("Formato de e-mail inválido."));

        return Result.Success(new Email(cleanEmail));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Endereco.ToLowerInvariant();
    }
}
