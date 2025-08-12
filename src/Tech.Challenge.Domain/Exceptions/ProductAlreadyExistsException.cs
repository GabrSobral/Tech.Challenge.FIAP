namespace Tech.Challenge.Domain.Exceptions;

public class ProductAlreadyExistsException : Exception
{
    public ProductAlreadyExistsException(Guid productId)
        : base($"Produto com ID {productId} já existe.")
    {
    }
}
