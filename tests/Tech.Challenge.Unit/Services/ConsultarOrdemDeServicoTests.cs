using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.OrdemServico.ValueObjects;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ConsultarOrdemDeServico;

using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class ConsultarOrdemDeServicoTests
{
    private readonly ILogger<ConsultarOrdemDeServicoService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<ConsultarOrdemDeServicoService>();

    [Fact]
    public async Task Execute_ShouldReturnSuccess_WhenValidCpfAndOrdersExist()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("joao@email.com").Value);
        var ordemServicoId = Guid.NewGuid();

        var servico = Servico.Criar("Troca de Óleo", 150.00m).Value;
        var produto = Produto.Criar("Óleo de Motor", 50, 10.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.LITROS).Value;

        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            cliente.Cpf,
            [ServicoOS.Criar(servico.Id, ordemServicoId, 150)],
            [ProdutoOS.Criar(produto.Id, ordemServicoId, 2, produto.PrecoUnitario)],
            ordemServicoId
        ).Value;

        var ordemRepo = new InMemoryOrdemServicoRepository();
        var clienteRepo = new InMemoryClienteRepository();

        ordemRepo.Dados.Add(ordemServico);
        clienteRepo.Dados.Add(cliente);

        var service = new ConsultarOrdemDeServicoService(_logger, ordemRepo, clienteRepo);

        var request = new Request(cliente.Cpf.ToString(), null);

        // Act
        var result = await service.Execute(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
    }

    [Fact]
    public async Task Execute_ShouldFail_WhenCpfIsInvalid()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var service = new ConsultarOrdemDeServicoService(_logger, new InMemoryOrdemServicoRepository(), new InMemoryClienteRepository());

        var request = new Request("123", null); // invalid CPF

        // Act
        var result = await service.Execute(request, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("CPF", result.Error!.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Execute_ShouldFail_WhenClienteNotFound()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var service = new ConsultarOrdemDeServicoService(_logger, new InMemoryOrdemServicoRepository(), new InMemoryClienteRepository());

        var request = new Request("51363622862", null); // valid CPF but no client

        // Act
        var result = await service.Execute(request, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Cliente", result.Error!.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Execute_ShouldFail_WhenOrdemServicoIdNotFound()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "Maria", Email.Criar("maria@email.com").Value);

        var clienteRepo = new InMemoryClienteRepository();
        clienteRepo.Dados.Add(cliente);

        var ordemRepo = new InMemoryOrdemServicoRepository();
        var service = new ConsultarOrdemDeServicoService(_logger, ordemRepo, clienteRepo);

        var request = new Request(cliente.Cpf.ToString(), Guid.NewGuid());

        // Act
        var result = await service.Execute(request, cancellationToken);

        // Assert
        Assert.True(result.IsFailure);
        Assert.Contains("Ordem de serviço", result.Error!.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Execute_ShouldReturnSingleOrder_WhenSearchingById()
    {
        // Arrange
        var cancellationToken = new CancellationToken();
        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "Pedro", Email.Criar("pedro@email.com").Value);

        var servico = Servico.Criar("Balanceamento", 80.00m).Value;
        var produto = Produto.Criar("Pneu", 20, 200.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;

        var ordemServicoId = Guid.NewGuid();
        var ordemServico = OrdemServico.Criar(
            cliente.Id,
            cliente.Cpf,
            [ServicoOS.Criar(servico.Id, ordemServicoId, 80)],
            [ProdutoOS.Criar(produto.Id, ordemServicoId, 4, produto.PrecoUnitario)],
            ordemServicoId
        ).Value;

        var ordemRepo = new InMemoryOrdemServicoRepository();
        var clienteRepo = new InMemoryClienteRepository();

        ordemRepo.Dados.Add(ordemServico);
        clienteRepo.Dados.Add(cliente);

        var service = new ConsultarOrdemDeServicoService(_logger, ordemRepo, clienteRepo);

        var request = new Request(cliente.Cpf.ToString(), ordemServicoId);

        // Act
        var result = await service.Execute(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Single(result.Value);
        Assert.Equal(ordemServicoId, result.Value.First().Id);
    }
}
