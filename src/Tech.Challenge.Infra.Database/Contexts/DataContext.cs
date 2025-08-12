using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Entities.OrdemServico.ValueObjects;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.ProdutoOS;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Entities.ServicoOS;
using Tech.Challenge.Domain.Entities.Usuario;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Infra.Database.Contexts;

public class DataContext : DbContext
{
    public virtual DbSet<Cliente> Clientes { get; set; }

    public virtual DbSet<Veiculo> Veiculos { get; set; }

    public virtual DbSet<OrdemServico> OrdemServicos { get; set; }

    public virtual DbSet<Produto> Produtos { get; set; }

    public virtual DbSet<Servico> Servicos { get; set; }

    public virtual DbSet<ServicoOS> ServicosOrdemServico { get; set; }

    public virtual DbSet<ProdutoOS> ProdutosOrdemServico{ get; set; }

    public virtual DbSet<Usuario> Usuarios { get; set; }

    public DataContext() { }

    public DataContext(DbContextOptions<DataContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Usuario>(e => 
        {
            e.Property(u => u.Email)
                .HasConversion(x => x.Endereco, x => Email.Criar(x).Value)
                .HasMaxLength(256);
        });
        modelBuilder.Entity<Cliente>(e =>
        {
            e.Property(c => c.Cpf)
                .HasConversion(x => x.Valor, x => CPF.Criar(x).Value)
                .HasMaxLength(16);

            e.Property(c => c.Email)
                .HasConversion(x => x.Endereco, x => Email.Criar(x).Value)
                .HasMaxLength(256);
        });

        modelBuilder.Entity<Veiculo>(e =>
        {
            e.Property(v => v.Placa)
                .HasConversion(x => x.Valor, x => Placa.Criar(x).Value)
                .HasMaxLength(8);
        });  

        modelBuilder.Entity<Produto>(e =>
        {
            e.Property(p => p.Tipo)
                .HasConversion<string>();

            e.Property(p => p.UnidadeMedida)
                .HasConversion<string>();
        });

        modelBuilder.Entity<OrdemServico>(e =>
        {
            e.Property(o => o.ClienteCpf)
                .HasConversion(x => x.Valor, x => CPF.Criar(x).Value)
                .HasMaxLength(16);

            e.Property(o => o.Status)
                .HasConversion<string>();

            e.Property(o => o.Orcamento)
                .HasConversion(v => v.Valor, v => new Orcamento(v));
        });
    }
}
