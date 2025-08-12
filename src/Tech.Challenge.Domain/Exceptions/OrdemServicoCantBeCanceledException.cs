namespace Tech.Challenge.Domain.Exceptions;

public class OrdemServicoCantBeCanceledException : Exception
{
    public OrdemServicoCantBeCanceledException()
        : base("Não é possível cancelar a ordem de serviço depois de aprovada... Visite a oficina mecânica presencialmente para cancelar o pedido.")
    {
    }
}
