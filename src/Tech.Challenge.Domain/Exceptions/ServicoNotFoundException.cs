namespace Tech.Challenge.Domain.Exceptions;

public class ServicoNotFoundException : Exception
{
    public ServicoNotFoundException(Guid servicoId) : base($"Serviço não encontrado: {servicoId}") { }
}