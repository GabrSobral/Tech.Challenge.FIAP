using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Domain.Exceptions;

public class VeiculoNotFoundException : Exception
{
    public VeiculoNotFoundException(Placa placa) : base($"Veículo não encontrado: {placa.Valor}") { }
    public VeiculoNotFoundException(Guid veiculoId) : base($"Veículo não encontrado: {veiculoId}") { }
    public VeiculoNotFoundException(Exception innerException) : base("Veículo não encontrado", innerException) { }
}
