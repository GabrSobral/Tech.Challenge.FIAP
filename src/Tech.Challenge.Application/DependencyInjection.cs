using Microsoft.Extensions.DependencyInjection;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Application.Core;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ConsultarOrdemDeServico;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.Cliente.AtualizarCliente;
using Tech.Challenge.Application.Services.Administrativo.Cliente.CadastrarCliente;
using Tech.Challenge.Application.Services.Administrativo.Cliente.DeletarCliente;
using Tech.Challenge.Application.Services.Administrativo.Cliente.IdentificarCliente;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.DeletarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico;
using Tech.Challenge.Application.Services.Administrativo.Produtos.AdicionarProduto;
using Tech.Challenge.Application.Services.Administrativo.Produtos.AtualizarProduto;
using Tech.Challenge.Application.Services.Administrativo.Produtos.DeletarProduto;
using Tech.Challenge.Application.Services.Administrativo.Produtos.ListarProdutos;
using Tech.Challenge.Application.Services.Administrativo.Servicos.AdicionarServico;
using Tech.Challenge.Application.Services.Administrativo.Servicos.AtualizarServico;
using Tech.Challenge.Application.Services.Administrativo.Servicos.DeletarServico;
using Tech.Challenge.Application.Services.Administrativo.Servicos.ListarServicos;
using Tech.Challenge.Application.Services.Administrativo.Usuario.AutenticarUsuario;
using Tech.Challenge.Application.Services.Administrativo.Usuario.RegistrarUsuario;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.AtualizarVeiculo;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.CadastrarVeiculo;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.ConsultarVeiculoPelaPlaca;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.DeletarVeiculo;
using Tech.Challenge.Domain.Interfaces;

namespace Tech.Challenge.Application;

/// <summary>
/// Contains the extensions method for registering dependencies in the DI framework.
/// </summary>
public static class DependencyInjection
{
    /// <summary>
    /// Registers the necessary services with the DI framework.
    /// </summary>
    /// <param name="services">The service collection.</param>
    /// <returns>The same service collection.</returns>
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IJsonWebToken, JsonWebToken>();
        services.AddSingleton<IPasswordEncrypter, PasswordEncrypter>();

        // Gestão da OS
        services.AddScoped<IdentificarClienteService>();
        services.AddScoped<CadastrarClienteService>();
        services.AddScoped<DeletarClienteService>();
        services.AddScoped<AtualizarClienteService>();
        
        services.AddScoped<CriarOrdemDeServicoService>();
        services.AddScoped<AtualizarStatusOrdemDeServico>();
        services.AddScoped<AtualizarOrdemDeServicoService>();
        services.AddScoped<DeletarOrdemDeServicoService>();
        services.AddScoped<MonitorarOrdensServicoService>();
        
        services.AddScoped<CadastrarVeiculoService>();
        services.AddScoped<DeletarVeiculoService>();
        services.AddScoped<AtualizarVeiculoService>();
        services.AddScoped<ConsultarVeiculoPelaPlacaService>();

        services.AddScoped<DeletarProdutoService>();
        services.AddScoped<AdicionarProdutoService>();
        services.AddScoped<ListarProdutosService>();
        services.AddScoped<AtualizarProdutoService>();

        services.AddScoped<ListarServicosService>();
        services.AddScoped<AtualizarServicoService>();
        services.AddScoped<DeletarServicoService>();
        services.AddScoped<AdicionarServicoService>();

        services.AddScoped<AutenticarUsuarioService>();
        services.AddScoped<RegistrarUsuarioService>();


        // Acompanhamento da OS
        services.AddScoped<AprovarOrdemDeServicoService>();
        services.AddScoped<ReprovarOrdemDeServicoService>();
        services.AddScoped<ConsultarOrdemDeServicoService>();

        return services;
    }
}
