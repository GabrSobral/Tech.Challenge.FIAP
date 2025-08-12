namespace Tech.Challenge.Application.Services.Administrativo.Veiculo.ConsultarVeiculoPelaPlaca;

public record Request(string Placa);

public record Response(
    Guid Id,
    string Placa,
    string Modelo,
    uint Ano);
