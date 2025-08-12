using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Application.Services.Administrativo.Cliente.IdentificarCliente;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Unit.InMemoryRepositories;


namespace Tech.Challenge.Unit.Services;

public class MonitorarOrdensServicoServiceTests
{
    private readonly ILogger<MonitorarOrdensServicoService> _logger =
        LoggerFactory.Create(builder => { }).CreateLogger<MonitorarOrdensServicoService>();

    [Fact]
    public async Task Execute_Should_Return_OrdensServico_With_Produtos_And_Servicos()
    {
        var cancellationToken = new CancellationToken();

        // Arrange: create repository and seed OS with products and services
        var inMemoryOrdemServicoRepository = new InMemoryOrdemServicoRepository();

        var cliente1 = Cliente.Criar(CPF.Criar("51363622862").Value, "João da Silva", Email.Criar("joao@email.com").Value); 
        var veiculo1 = Veiculo.Criar(Placa.Criar("ABC-1234").Value, "BYD King GS", 2024, cliente1.Id);

        var ordemServico = OrdemServico.Criar(
            cliente1.Id,
            cliente1.Cpf,
            [],
            [],
            null
        ).Value;

        // Adiciona serviços
        var servico1 = Servico.Criar("Troca de óleo", 150).Value;
        var servico2 = Servico.Criar("Alinhamento", 100).Value;

        ordemServico.Servicos = new List<ServicoOS> 
        { 
            ServicoOS.Criar(servico1.Id, ordemServico.Id, servico1.PrecoServico), 
            ServicoOS.Criar(servico2.Id, ordemServico.Id, servico2.PrecoServico),
        };

        // Adiciona produtos
        var produto1 = Produto.Criar("Filtro de óleo", 50, 20, ETipoProduto.PECA, EUnidadeMedida.UNIDADE).Value;
        var produto2 = Produto.Criar("Pneu", 30, 230, ETipoProduto.PECA, EUnidadeMedida.UNIDADE).Value;

        ordemServico.Produtos = new List<ProdutoOS> 
        { 
            ProdutoOS.Criar(produto1.Id, ordemServico.Id, 2, produto1.PrecoUnitario),
            ProdutoOS.Criar(produto2.Id, ordemServico.Id, 4, produto2.PrecoUnitario)
        };

        ordemServico.Produtos.ElementAt(0).Produto = produto1;
        ordemServico.Produtos.ElementAt(1).Produto = produto2;

        ordemServico.Servicos.ElementAt(0).Servico = servico1;
        ordemServico.Servicos.ElementAt(1).Servico = servico2;

        await inMemoryOrdemServicoRepository.AddOrdemServico(ordemServico, cancellationToken);

        var service = new MonitorarOrdensServicoService(_logger, inMemoryOrdemServicoRepository);
        var request = new Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico.Request(1, 50);

        // Act
        var result = await service.Execute(request, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess, "O resultado deve ser bem-sucedido.");
        Assert.NotNull(result.Value);
        Assert.NotEmpty(result.Value.OrdensServico);
        Assert.All(result.Value.OrdensServico, os =>
        {
            Assert.NotEmpty(os.Produtos);
            Assert.NotEmpty(os.Servicos);
        });
    }
}
