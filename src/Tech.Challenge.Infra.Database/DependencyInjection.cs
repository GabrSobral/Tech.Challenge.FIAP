using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using Tech.Challenge.Infra.Database.Contexts;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Repositories;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Infra.Database.Utils;

namespace Tech.Challenge.Infra.Database;

public static class DependenyInjection
{
    public static IServiceCollection AddInfraDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IClienteRepository, ClienteRepository>();
        services.AddScoped<IVeiculoRepository, VeiculoRepository>();
        services.AddScoped<IOrdemServicoRepository, OrdemServicoRepository>();
        services.AddScoped<IServicoRepository, ServicoRepository>();
        services.AddScoped<IProdutoRepository, ProdutoRepository>();
        services.AddScoped<IUsuarioRepository, UsuarioRepository>();

        services.AddScoped<IUnitOfWork, UnitOfWork>();

        string connectionString = configuration.GetConnectionString("PostgreSql")!;

        services.AddDbContext<DataContext>(options => options.UseNpgsql(connectionString));

        return services;
    }
}

