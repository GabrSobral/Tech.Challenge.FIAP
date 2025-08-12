using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Domain.Exceptions;

public class ClienteCpfMismatchException : Exception
{
    public ClienteCpfMismatchException(): base("O CPF do cliente não corresponde ao CPF da ordem de serviço.")
    {
    }

    public ClienteCpfMismatchException(CPF clienteCpf, CPF ordemServicoCpf)
        : base($"O CPF do cliente '{clienteCpf.Valor}' não corresponde ao CPF da ordem de serviço '{ordemServicoCpf.Valor}'.")
    {
    }
}
