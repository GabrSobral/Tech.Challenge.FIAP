using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Domain.Exceptions;

public class VeiculoAlreadyRegisteredException : Exception
{
    public VeiculoAlreadyRegisteredException(Placa placa) : base($"Veículo já cadastrado com a placa: {placa.Valor}")
    {
    }
}
