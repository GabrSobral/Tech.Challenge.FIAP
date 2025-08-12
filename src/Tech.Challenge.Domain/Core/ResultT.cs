namespace Tech.Challenge.Domain.Core;

public class Result<T> : Result
{
    public T Value { get; }

    // Construtor protegido para manter o encapsulamento.
    // Usa o construtor da classe base para inicializar as propriedades comuns.
    protected internal Result(bool isSuccess, T value, Exception? error)
        : base(isSuccess, error)
    {
        // Validação: um resultado de falha não pode ter um valor (diferente do padrão)
        if (!isSuccess && !EqualityComparer<T>.Default.Equals(value, default!))
        {
            throw new InvalidOperationException("Um resultado de falha não pode conter um valor.");
        }

        Value = value;
    }
}
