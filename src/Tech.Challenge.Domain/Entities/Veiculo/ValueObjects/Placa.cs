namespace Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

using System.Collections.Generic;
using System.Text.RegularExpressions;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;

public sealed class Placa : ValueObject<Placa>
{
    private static readonly Regex PlacaAntigaRegex = new Regex(@"^[A-Z]{3}-?\d{4}$", RegexOptions.IgnoreCase);
    private static readonly Regex PlacaMercosulRegex = new Regex(@"^[A-Z]{3}\d[A-Z]\d{2}$", RegexOptions.IgnoreCase);

    public string Valor { get; }

    private Placa(string valor)
    {
        Valor = valor.ToUpperInvariant();
    }

    public static Result<Placa> Criar(string placa)
    {
        if (string.IsNullOrWhiteSpace(placa))
            return Result.Failure<Placa>(new DomainError("Placa não pode ser nula ou vazia."));

        var valorLimpo = placa.ToUpper().Replace("-", "").Trim();

        if (!PlacaAntigaRegex.IsMatch(valorLimpo) && !PlacaMercosulRegex.IsMatch(valorLimpo))
            return Result.Failure<Placa>(new DomainError("Formato de placa inválido."));

        return Result.Success(new Placa(valorLimpo));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Valor;
    }

    public override string ToString()
    {
        // Exibe no formato ABC-1234 se for a placa antiga
        if (PlacaAntigaRegex.IsMatch(Valor))
            return Valor.Substring(0, 3) + "-" + Valor.Substring(3, 4);

        return Valor;
    }
}

