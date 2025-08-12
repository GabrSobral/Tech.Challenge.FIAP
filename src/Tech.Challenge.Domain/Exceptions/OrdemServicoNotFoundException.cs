namespace Tech.Challenge.Domain.Exceptions;

public class OrdemServicoNotFoundException : Exception
{
    public OrdemServicoNotFoundException(Guid Id) : base($"Ordem de serviço não encontrada: {Id}")
    {
    }
}
