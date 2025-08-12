using System.Text.RegularExpressions;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;

namespace Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

public sealed class CPF : ValueObject<CPF>
{
    public string Valor { get; }

    private CPF(string valor)
    {
        Valor = valor;
    }

    public static Result<CPF> Criar(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return Result.Failure<CPF>(new DomainError("CPF não pode ser vazio."));

        var numeros = Regex.Replace(cpf, @"[^\d]", "");

        if (numeros.Length != 11 || !ValidarCPF(numeros))
            return Result.Failure<CPF>(new DomainError("CPF inválido."));

        return Result.Success(new CPF(numeros));
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Valor;
    }

    private static bool ValidarCPF(string cpf)
    {
        var invalidos = new[]
        {
            "00000000000", "11111111111", "22222222222",
            "33333333333", "44444444444", "55555555555",
            "66666666666", "77777777777", "88888888888", 
            "99999999999"
        };

        if (Array.Exists(invalidos, i => i == cpf))
            return false;

        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += (cpf[i] - '0') * (10 - i);

        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += (cpf[i] - '0') * (11 - i);

        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        return cpf[9] - '0' == digito1 && cpf[10] - '0' == digito2;
    }

    public override string ToString()
    {
        return Convert.ToUInt64(Valor).ToString(@"000\.000\.000\-00");
    }
}
