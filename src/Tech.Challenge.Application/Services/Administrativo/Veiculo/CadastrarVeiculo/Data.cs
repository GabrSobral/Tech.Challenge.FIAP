namespace Tech.Challenge.Application.Services.Administrativo.Veiculo.CadastrarVeiculo;

public record Request(
    string Modelo,
    string Placa,
    uint Ano,
    Guid ClienteId);

public record Response(Guid VeiculoId);
