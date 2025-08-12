using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.CadastrarVeiculo;
using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class CadastrarVeiculoTests
{
    private readonly ILogger<CadastrarVeiculoService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<CadastrarVeiculoService>();

    [Fact]
    public async Task Execute_Should_RegisterVehicle_WhenPlacaIsValid()
    {   
        var cancellationToken = new CancellationToken();

        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();

        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("joao@email.com").Value);

        inMemoryClienteRepository.Dados.Add(cliente1);

        var service = new CadastrarVeiculoService(_logger, inMemoryVeiculoRepository, inMemoryClienteRepository, new UnitOfWorkMock());

        var request = new Request("BYD King GS", "ABC-1234", 2024, cliente1.Id);
        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsSuccess, "O resultado deve ser bem-sucedido.");
        Assert.NotNull(result.Value);
    }

    [Fact]
    public async Task Execute_Should_ThrowAndDomainError_WhenPlacaIsInvalid()
    {
        var cancellationToken = new CancellationToken();

        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();

        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("joao@email.com").Value);

        inMemoryClienteRepository.Dados.Add(cliente1);

        var service = new CadastrarVeiculoService(_logger, inMemoryVeiculoRepository, inMemoryClienteRepository, new UnitOfWorkMock());

        var request = new Request("BYD King GS", "123-1234", 2024, cliente1.Id);
        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure, "O resultado deve ser mal-sucedido.");
        Assert.Null(result.Value);
        Assert.IsType<DomainError>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_ThrowAndDomainError_WhenPlacaIsAlreadyRegistered()
    {
        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("joao@email.com").Value);
        var veiculo = Veiculo.Criar(Placa.Criar("ABC-1234").Value, "BYD King GS", 2024, cliente1.Id);

        var cancellationToken = new CancellationToken();

        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();

        inMemoryVeiculoRepository.Dados.Add(veiculo);
        inMemoryClienteRepository.Dados.Add(cliente1);

        var service = new CadastrarVeiculoService(_logger, inMemoryVeiculoRepository, inMemoryClienteRepository, new UnitOfWorkMock());

        var request = new Request("BYD King GS", "ABC-1234", 2024, cliente1.Id);
        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure, "O resultado deve ser mal-sucedido.");
        Assert.Null(result.Value);
        Assert.IsType<VeiculoAlreadyRegisteredException>(result.Error);
    }
}
