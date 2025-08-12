namespace Tech.Challenge.Domain.Exceptions;

public class ClienteAlreadyRegisteredException : Exception
{
    public ClienteAlreadyRegisteredException(string cpf)
        : base($"Cliente já cadastrado com o CPF: {cpf}")
    {
    }
    public ClienteAlreadyRegisteredException(string cpf, Exception innerException)
        : base($"Cliente já cadastrado com o CPF: {cpf}", innerException)
    {
    }
}
