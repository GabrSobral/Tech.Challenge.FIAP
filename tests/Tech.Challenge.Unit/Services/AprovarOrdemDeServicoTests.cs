using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class AprovarOrdemDeServicoTests
{
    private readonly ILogger<AprovarOrdemDeServicoService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<AprovarOrdemDeServicoService>();

    private readonly CancellationToken _cancellationToken = new();

    private readonly UnitOfWorkMock _unitOfWork = new();

    [Fact]
    public async Task Execute_Should_Succeed_When_ValidRequest()
    {
        // Arrange
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);
        var ordemServicoId = Guid.NewGuid();

        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            cliente.Cpf,
            servicos: [],
            produtos: [],
            id: ordemServicoId).Value;

        // Move status to AWAITING_APPROVAL so Aprovar() can succeed
        ordemServico.Status = EServiceOrderStatus.AWAITING_APPROVAL;

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);
        await ordemServicoRepo.AddOrdemServico(ordemServico, _cancellationToken);

        var service = new AprovarOrdemDeServicoService(_logger, ordemServicoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, ordemServicoId);

        // Act
        var result = await service.Execute(request, _cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(EServiceOrderStatus.IN_PROGRESS, ordemServico.Status);
    }

    [Fact]
    public async Task Execute_Should_Fail_When_ClienteNotFound()
    {
        // Arrange
        var clienteCpf = CPF.Criar("51363622862").Value;
        var ordemServicoId = Guid.NewGuid();

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();

        var service = new AprovarOrdemDeServicoService(_logger, ordemServicoRepo, clienteRepo, _unitOfWork);

        var request = new Request(clienteCpf, ordemServicoId);

        // Act
        var result = await service.Execute(request, _cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ClienteNotFoundException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_When_OrdemServicoNotFound()
    {
        // Arrange
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);

        var service = new AprovarOrdemDeServicoService(_logger, ordemServicoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, Guid.NewGuid()); // Random ordemServicoId

        // Act
        var result = await service.Execute(request, _cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<OrdemServicoNotFoundException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_When_CpfMismatch()
    {
        // Arrange
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);
        var outroCpf = CPF.Criar("98765432100").Value;
        var ordemServicoId = Guid.NewGuid();

        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            outroCpf,  // Different CPF here
            servicos: [],
            produtos: [],
            id: ordemServicoId).Value;

        ordemServico.Status = EServiceOrderStatus.AWAITING_APPROVAL;

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);
        await ordemServicoRepo.AddOrdemServico(ordemServico, _cancellationToken);

        var service = new AprovarOrdemDeServicoService(_logger, ordemServicoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, ordemServicoId);

        // Act
        var result = await service.Execute(request, _cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.IsType<ClienteCpfMismatchException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_When_AprovarReturnsFailure()
    {
        // Arrange
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);
        var ordemServicoId = Guid.NewGuid();

        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            cliente.Cpf,
            servicos: [],
            produtos: [],
            id: ordemServicoId).Value;

        // Set status to something that makes Aprovar fail (e.g. RECEIVED)
        ordemServico.Status = EServiceOrderStatus.RECEIVED;

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);
        await ordemServicoRepo.AddOrdemServico(ordemServico, _cancellationToken);

        var service = new AprovarOrdemDeServicoService(_logger, ordemServicoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, ordemServicoId);

        // Act
        var result = await service.Execute(request, _cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
    }
}
