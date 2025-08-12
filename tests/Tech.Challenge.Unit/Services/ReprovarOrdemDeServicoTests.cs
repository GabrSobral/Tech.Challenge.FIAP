using Microsoft.Extensions.Logging;
using System.Threading;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico;
using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class ReprovarOrdemDeServicoTests
{
    private readonly ILogger<ReprovarOrdemDeServicoService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<ReprovarOrdemDeServicoService>();

    private readonly CancellationToken _cancellationToken = new();

    private readonly UnitOfWorkMock _unitOfWork = new();

    [Fact]
    public async Task Execute_Should_Succeed_When_ValidRequestAndStatusAwaitingApproval()
    {
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);

        var ordemServicoId = Guid.NewGuid();

        var produto = Produto.Criar("Óleo", 100, 10m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.LITROS);
        Assert.True(produto.IsSuccess);

        var produtoOS = ProdutoOS.Criar(produto.Value.Id, ordemServicoId, 5, produto.Value.PrecoUnitario);

        produtoOS.Produto = produto.Value;

        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            cliente.Cpf,
            servicos: Array.Empty<ServicoOS>(),
            produtos: new[] { produtoOS },
            id: ordemServicoId).Value;

        ordemServico.Status = Domain.Enums.EServiceOrderStatus.AWAITING_APPROVAL;

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();
        var produtoRepo = new InMemoryProdutoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);
        await ordemServicoRepo.AddOrdemServico(ordemServico, _cancellationToken);
        await produtoRepo.AddAsync(produto.Value, _cancellationToken);

        var service = new ReprovarOrdemDeServicoService(_logger, ordemServicoRepo, produtoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, ordemServicoId);

        // Act
        var result = await service.Execute(request, _cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(Domain.Enums.EServiceOrderStatus.CANCELED, ordemServico.Status);
        // Check if stock was restored
        var updatedProduto = await produtoRepo.GetByIdAsync(produto.Value.Id, _cancellationToken);
        Assert.Equal((uint)(100 + 5), updatedProduto!.Quantidade); // initial 100 + 5 restored
    }

    [Fact]
    public async Task Execute_Should_Fail_When_ClienteNotFound()
    {
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);
        var ordemServicoId = Guid.NewGuid();

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();
        var produtoRepo = new InMemoryProdutoRepository();

        var service = new ReprovarOrdemDeServicoService(_logger, ordemServicoRepo, produtoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, ordemServicoId);

        var result = await service.Execute(request, _cancellationToken);

        Assert.True(result.IsFailure);
        Assert.IsType<ClienteNotFoundException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_When_OrdemServicoNotFound()
    {
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();
        var produtoRepo = new InMemoryProdutoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);

        var service = new ReprovarOrdemDeServicoService(_logger, ordemServicoRepo, produtoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, Guid.NewGuid());

        var result = await service.Execute(request, _cancellationToken);

        Assert.True(result.IsFailure);
        Assert.IsType<OrdemServicoNotFoundException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_When_StatusIsNotAwaitingApproval()
    {
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("joao@email.com").Value);
 
        var ordemServicoId = Guid.NewGuid();

        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            cliente.Cpf,
            servicos: Array.Empty<ServicoOS>(),
            produtos: Array.Empty<ProdutoOS>(),
            id: ordemServicoId).Value;

        // Status other than AWAITING_APPROVAL
        ordemServico.Status = Domain.Enums.EServiceOrderStatus.IN_PROGRESS;

        var clienteRepo = new InMemoryClienteRepository();
        var ordemServicoRepo = new InMemoryOrdemServicoRepository();
        var produtoRepo = new InMemoryProdutoRepository();

        await clienteRepo.AddCliente(cliente, _cancellationToken);
        await ordemServicoRepo.AddOrdemServico(ordemServico, _cancellationToken);

        var service = new ReprovarOrdemDeServicoService(_logger, ordemServicoRepo, produtoRepo, clienteRepo, _unitOfWork);

        var request = new Request(cliente.Cpf, ordemServicoId);

        var result = await service.Execute(request, _cancellationToken);

        Assert.True(result.IsFailure);
        Assert.IsType<OrdemServicoCantBeCanceledException>(result.Error);
    }
}
