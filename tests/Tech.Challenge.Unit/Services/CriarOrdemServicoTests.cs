using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico;
using Tech.Challenge.Unit.InMemoryRepositories;

namespace Tech.Challenge.Unit.Services;

public class CriarOrdemServicoTests
{
    private readonly ILogger<CriarOrdemDeServicoService> _logger = LoggerFactory.Create(builder => { }).CreateLogger<CriarOrdemDeServicoService>();
    private readonly IUnitOfWork _unitOfWork = new UnitOfWorkMock();

    [Fact]
    public async Task Execute_Should_CreateAServiceOrder_WhenTheInputIsValid()
    {
        var cancellationToken = new CancellationToken();

        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("johndoe@email.com").Value);

        var servico1 = Servico.Criar("Troca de Óleo", 150.00m);
        var servico2 = Servico.Criar("Alinhamento", 100.00m);
        var servico3 = Servico.Criar("Balanceamento", 80.00m);
        var servico4 = Servico.Criar("Lavagem", 50.00m);

        var veiculo1 = Veiculo.Criar(Placa.Criar("ABC-1234").Value, "BYD King GS", 2024, cliente1.Id);

        var produto1 = Produto.Criar("Óleo de Motor", 50, 10.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.LITROS).Value;
        var produto2 = Produto.Criar("Filtro de Óleo", 30, 5.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;
        var produto3 = Produto.Criar("Pneu", 20, 200.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;
        var produto4 = Produto.Criar("Vela de Ignição", 100, 2.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;
        var produto5 = Produto.Criar("Filtro de Ar", 40, 15.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;

        var inMemoryServicoRepository = new InMemoryServicoRepository();
        var inMemoryOrdemServicoRepository = new InMemoryOrdemServicoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();
        var inMemoryProdutoRepository = new InMemoryProdutoRepository();

        inMemoryServicoRepository.Dados.AddRange([
            servico1.Value,
            servico2.Value,
            servico3.Value,
            servico4.Value
        ]);

        inMemoryClienteRepository.Dados.AddRange([cliente1]);

        inMemoryVeiculoRepository.Dados.Add(veiculo1);

        inMemoryProdutoRepository.Dados.AddRange([
            produto1,
            produto2,
            produto3,
            produto4,
            produto5
        ]);

        var service = new CriarOrdemDeServicoService(_logger, inMemoryServicoRepository, inMemoryOrdemServicoRepository, inMemoryProdutoRepository, inMemoryClienteRepository, _unitOfWork);

        var request = new Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico.Request(
            cliente1.Id, 
            veiculo1.Id, 
            [servico1.Value.Id, servico2.Value.Id],
            [
                new ProdutosRequest(produto1.Id, 1), 
                new ProdutosRequest(produto2.Id, 2),
                new ProdutosRequest(produto3.Id, 1),
                new ProdutosRequest(produto4.Id, 4),
            ]
        );

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsSuccess, "O resultado deve ser bem-sucedido.");
        Assert.NotNull(result.Value);
    }



    [Fact]
    public async Task Execute_Should_ThrowAdnError_WhenTheAnProductDontExists()
    {
        var cancellationToken = new CancellationToken();

        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("johndoe@email.com").Value);

        var servico1 = Servico.Criar("Troca de Óleo", 150.00m);
        var servico2 = Servico.Criar("Alinhamento", 100.00m);
        var servico3 = Servico.Criar("Balanceamento", 80.00m);
        var servico4 = Servico.Criar("Lavagem", 50.00m);

        var veiculo1 = Veiculo.Criar(Placa.Criar("ABC-1234").Value, "BYD King GS", 2024, cliente1.Id);

        var produto1 = Produto.Criar("Óleo de Motor", 50, 10.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.LITROS).Value;
        var produto2 = Produto.Criar("Filtro de Óleo", 30, 5.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;
        var produto3 = Produto.Criar("Pneu", 20, 200.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;
        var produto4 = Produto.Criar("Vela de Ignição", 100, 2.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;
        var produto5 = Produto.Criar("Filtro de Ar", 40, 15.00m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;

        var inMemoryServicoRepository = new InMemoryServicoRepository();
        var inMemoryOrdemServicoRepository = new InMemoryOrdemServicoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryVeiculoRepository = new InMemoryVeiculoRepository();
        var inMemoryProdutoRepository = new InMemoryProdutoRepository();

        inMemoryServicoRepository.Dados.AddRange([
            servico1.Value,
            servico2.Value,
            servico3.Value,
            servico4.Value
        ]);

        inMemoryClienteRepository.Dados.AddRange([cliente1]);

        inMemoryVeiculoRepository.Dados.Add(veiculo1);

        inMemoryProdutoRepository.Dados.AddRange([
            produto1,
            produto2,
            produto3,
            produto4,
            produto5
        ]);

        var service = new CriarOrdemDeServicoService(_logger, inMemoryServicoRepository, inMemoryOrdemServicoRepository, inMemoryProdutoRepository, inMemoryClienteRepository, _unitOfWork);

        var request = new Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico.Request(
            cliente1.Id,
            veiculo1.Id,
            [servico1.Value.Id, servico2.Value.Id],
            [
                new ProdutosRequest(Guid.NewGuid(), 1), // The wrong Product ID
                new ProdutosRequest(produto2.Id, 2),
                new ProdutosRequest(produto3.Id, 1),
                new ProdutosRequest(produto4.Id, 4),
            ]
        );

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure, "O resultado deve ser mal-sucedido.");
        Assert.Null(result.Value);
        Assert.Empty(inMemoryOrdemServicoRepository.Dados);
    }

    [Fact]
    public async Task Execute_Should_Fail_WhenClienteDoesNotExist()
    {
        var cancellationToken = new CancellationToken();

        var inMemoryServicoRepository = new InMemoryServicoRepository();
        var inMemoryOrdemServicoRepository = new InMemoryOrdemServicoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryProdutoRepository = new InMemoryProdutoRepository();

        var service = new CriarOrdemDeServicoService(
            _logger,
            inMemoryServicoRepository,
            inMemoryOrdemServicoRepository,
            inMemoryProdutoRepository,
            inMemoryClienteRepository,
            _unitOfWork
        );

        var request = new Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico.Request(
            Guid.NewGuid(), // Cliente inexistente
            Guid.NewGuid(),
            [],
            []
        );

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure);
        Assert.IsType<ClienteNotFoundException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_WhenServicoDoesNotExist()
    {
        var cancellationToken = new CancellationToken();

        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "Fulano", Email.Criar("email@email.com").Value);
        var veiculo = Veiculo.Criar(Placa.Criar("XYZ-0001").Value, "Carro", 2020, cliente.Id);
        var produto = Produto.Criar("Óleo", 10, 50m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.LITROS).Value;

        var inMemoryServicoRepository = new InMemoryServicoRepository();
        var inMemoryOrdemServicoRepository = new InMemoryOrdemServicoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryProdutoRepository = new InMemoryProdutoRepository();

        inMemoryClienteRepository.Dados.Add(cliente);
        inMemoryProdutoRepository.Dados.Add(produto);

        var service = new CriarOrdemDeServicoService(
            _logger,
            inMemoryServicoRepository,
            inMemoryOrdemServicoRepository,
            inMemoryProdutoRepository,
            inMemoryClienteRepository,
            _unitOfWork
        );

        var request = new Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico.Request(
            cliente.Id,
            veiculo.Id,
            [Guid.NewGuid()], // Serviço inexistente
            [new ProdutosRequest(produto.Id, 1)]
        );

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure);
        Assert.IsType<ServicoNotFoundException>(result.Error);
    }

    [Fact]
    public async Task Execute_Should_Fail_WhenProductHasNoStock()
    {
        var cancellationToken = new CancellationToken();

        var cliente = Cliente.Criar(CPF.Criar("51363622862").Value, "Fulano", Email.Criar("email@email.com").Value);
        var veiculo = Veiculo.Criar(Placa.Criar("XYZ-0001").Value, "Carro", 2020, cliente.Id);
        var servico = Servico.Criar("Troca de Óleo", 100m);
        var produtoSemEstoque = Produto.Criar("Pneu", 1, 200m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE).Value;

        produtoSemEstoque.DiminuirEstoque(1); // Diminuindo o estoque para 0

        var inMemoryServicoRepository = new InMemoryServicoRepository();
        var inMemoryOrdemServicoRepository = new InMemoryOrdemServicoRepository();
        var inMemoryClienteRepository = new InMemoryClienteRepository();
        var inMemoryProdutoRepository = new InMemoryProdutoRepository();

        inMemoryServicoRepository.Dados.Add(servico.Value);
        inMemoryClienteRepository.Dados.Add(cliente);
        inMemoryProdutoRepository.Dados.Add(produtoSemEstoque);

        var service = new CriarOrdemDeServicoService(
            _logger,
            inMemoryServicoRepository,
            inMemoryOrdemServicoRepository,
            inMemoryProdutoRepository,
            inMemoryClienteRepository,
            _unitOfWork
        );

        var request = new Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico.Request(
            cliente.Id,
            veiculo.Id,
            [servico.Value.Id],
            [new ProdutosRequest(produtoSemEstoque.Id, 1)]
        );

        var result = await service.Execute(request, cancellationToken);

        Assert.True(result.IsFailure);
        Assert.NotNull(result.Error);
        Assert.Empty(inMemoryOrdemServicoRepository.Dados);
    }
}
