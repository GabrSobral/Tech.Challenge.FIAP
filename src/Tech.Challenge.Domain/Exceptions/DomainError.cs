namespace Tech.Challenge.Domain.Exceptions;

public class DomainError : Exception
{
    public DomainError() : base("Ocorreu um erro de domínio.") { }
    public DomainError(string message) : base(message) { }
    public DomainError(string message, Exception innerException) : base(message, innerException) { }
}
