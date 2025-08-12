namespace Tech.Challenge.Domain.Exceptions;

public class UsuarioAlreadyExistsException : Exception
{
    public UsuarioAlreadyExistsException(string message) : base(message)
    {
    }
    public UsuarioAlreadyExistsException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
