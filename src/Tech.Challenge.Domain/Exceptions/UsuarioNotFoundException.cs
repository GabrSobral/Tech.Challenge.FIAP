namespace Tech.Challenge.Domain.Exceptions;

public class UsuarioNotFoundException : Exception
{
    public UsuarioNotFoundException(string email)
        : base($"Usuário com o email '{email}' não encontrado.")
    {
    }
    public UsuarioNotFoundException(Guid usuarioId)
        : base($"Usuário com o ID '{usuarioId}' não encontrado.")
    {
    }
}
