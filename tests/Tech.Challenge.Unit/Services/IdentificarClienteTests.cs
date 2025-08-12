using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Application.Services.Administrativo.Cliente.IdentificarCliente;
using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class IdentificarClienteTests
{
    private readonly ILogger<IdentificarClienteService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<IdentificarClienteService>();

    [Fact]
    public async Task Execute_Should_IdentifyACustomer_WhenTheCpfIsValid()
    {
        var cancellationToken = new CancellationToken();

        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("joao@email.com").Value);

        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();

        await inMemoryClienteRepository.AddCliente(cliente1, cancellationToken); // Adding a customer with a valid CPF

        var service = new IdentificarClienteService(_logger, inMemoryClienteRepository, inMemoryVeiculoRepository);
        var request = new Request("51363622862");

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsSuccess, "O resultado deve ser bem-sucedido.");
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Execute_Should_ThrowAndDomainError_WhenTheCpfIsInvalid()
    {
        var cancellationToken = new CancellationToken();

        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();

        var service = new IdentificarClienteService(_logger, inMemoryClienteRepository, inMemoryVeiculoRepository);
        var request = new Request("12345678900");

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure, "O resultado deve ser mal-sucedido.");
        Assert.Null(result.Value);
        Assert.IsType<DomainError>(result.Error);
    }
}
