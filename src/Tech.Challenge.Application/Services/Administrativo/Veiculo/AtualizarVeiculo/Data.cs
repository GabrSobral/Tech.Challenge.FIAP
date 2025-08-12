using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Application.Services.Administrativo.Veiculo.AtualizarVeiculo;

public record Request(
    Guid VeiculoId,
    Placa Placa,
    string Modelo,
    uint Ano
);

public record RequestBody(
    string Placa,
    string Modelo,
    uint Ano
);
