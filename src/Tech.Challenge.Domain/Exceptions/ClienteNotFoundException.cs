using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Domain.Exceptions;

public class ClienteNotFoundException : Exception
{
    public ClienteNotFoundException() : base("Cliente não encontrado!") { }
    public ClienteNotFoundException(CPF cpf) : base($"Cliente não encontrado. CPF: {cpf.Valor}") { }

    public ClienteNotFoundException(Guid id) : base($"Cliente não encontrado. ID: {id}") { }

    public ClienteNotFoundException(Exception innerException) : base("Cliente não encontrado", innerException) { }
}
