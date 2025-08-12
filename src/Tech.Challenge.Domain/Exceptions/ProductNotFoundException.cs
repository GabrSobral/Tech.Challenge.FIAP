namespace Tech.Challenge.Domain.Exceptions;

public class ProductNotFoundException : Exception
{
    public ProductNotFoundException(Guid productId) : base($"Produto não encontrado: {productId}") { }
}
