using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.ConsultarVeiculoPelaPlaca;
using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class ConsultarVeiculoPelaPlacaTests
{
    private readonly ILogger<ConsultarVeiculoPelaPlacaService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<ConsultarVeiculoPelaPlacaService>();

    [Fact]
    public async Task Execute_Should_IdentifyACustomer_WhenTheCpfIsValid()
    {
        var cancellationToken = new CancellationToken();

        var veiculo1 = Veiculo.Criar(Placa.Criar("ABC-1234").Value, "BYD King GS", 2024, Guid.NewGuid());
          
        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();
        await inMemoryVeiculoRepository.AddVeiculoAsync(veiculo1, cancellationToken); // Adding a customer with a valid CPF

        var service = new ConsultarVeiculoPelaPlacaService(_logger, inMemoryVeiculoRepository);
        var request = new Request("ABC-1234");

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsSuccess, "O resultado deve ser bem-sucedido.");
        Assert.NotNull(result.Value);
    }
}
