using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.Produto;
using Tech.Challenge.Domain.Entities.Servico;
using Tech.Challenge.Domain.Entities.Usuario;
using Tech.Challenge.Domain.Entities.Veiculo;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Infra.Database.Contexts;

public class SeedData(DataContext dbContext)
{
    public void SeedProdutos()
    {
        dbContext.Produtos.AddRange([
            Produto.Criar("Óleo", 100, 10m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.LITROS, Guid.Parse("a1c4ae71-4a8c-4032-983a-4534331bbb7c")).Value,
            Produto.Criar("Filtro de Ar", 50, 15m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("5bee6278-ba09-4a90-bcf2-c79e0db42b8a")).Value,
            Produto.Criar("Pastilha de Freio", 30, 25m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("d7ce621c-d42d-4c30-9528-3020262c690b")).Value,
            Produto.Criar("Correia Dentada", 20, 50m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("e4388ba2-c257-4bfa-b595-94521ae6c361")).Value,
            Produto.Criar("Bateria", 15, 200m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("f642885d-42fb-413a-b9ad-be7a1a01be17")).Value,
            Produto.Criar("Velas de Ignição", 40, 5m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("24b1f2d5-3375-43ed-bdef-04286dca32f7")).Value,
            Produto.Criar("Amortecedor", 25, 150m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("c81942b1-409b-4151-9c4d-2d929ee7f6ce")).Value,
            Produto.Criar("Pneu", 60, 300m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("e9743fb0-5318-4215-b52e-a342ee2b954a")).Value,
            Produto.Criar("Lâmpada", 80, 10m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("db8de422-eb85-479d-804f-9d2d61dcf874")).Value,
            Produto.Criar("Radiador", 10, 400m, Challenge.Domain.Enums.ETipoProduto.INSUMO, Challenge.Domain.Enums.EUnidadeMedida.UNIDADE, Guid.Parse("a6d85808-53f7-40dd-b6f8-f05ef5063b15")).Value
        ]);
    }

    public void SeedServicos()
    {
        dbContext.Servicos.AddRange(new[]
        {
            Servico.Criar("Troca de embreagem", 1200m, Guid.Parse("0f9e8d7c-6b5a-4c3d-a2e1-9d8c7b6a0010")).Value,
            Servico.Criar("Troca de amortecedores", 700m, Guid.Parse("1a2b3c4d-5e6f-4a7b-8c9d-0e1f2a3b9009")).Value,
            Servico.Criar("Troca de pastilhas de freio", 200m, Guid.Parse("2d9c3b7e-1f5a-45c8-a2e9-4b3d2f6c4004")).Value,
            Servico.Criar("Alinhamento e balanceamento", 120m, Guid.Parse("3c1f4a5b-8d7a-4c8b-bff3-2e9f4d5c2002")).Value,
            Servico.Criar("Troca de correia dentada", 600m, Guid.Parse("4e6f3a2b-5d8c-41c9-b3f2-7a8d5c9e6006")).Value,
            Servico.Criar("Diagnóstico eletrônico", 90m, Guid.Parse("5d2c3e4f-8b9a-4f3d-a7c8-9b2d3e4f8008")).Value,
            Servico.Criar("Revisão completa", 450m, Guid.Parse("6e7d1f8c-4b9d-45af-92e3-8a7c4c8d3003")).Value,
            Servico.Criar("Troca de velas de ignição", 180m, Guid.Parse("7b9c4e3d-2f6a-4c8b-9d3f-1a2b3c4d7007")).Value,
            Servico.Criar("Troca de fluido de freio", 100m, Guid.Parse("8f3b1e2d-7c9a-48c4-b8f3-3e2c1d5a5005")).Value,
            Servico.Criar("Troca de óleo e filtro", 150m, Guid.Parse("9a6f2f47-5c8b-4b87-bb36-12f8c5a5a001")).Value,
            Servico.Criar("Limpeza de bicos injetores", 250m, Guid.Parse("0b1c2d3e-4f5a-6b7c-8d9e-0f1a2b3c4001")).Value,
            Servico.Criar("Substituição de bateria", 300m, Guid.Parse("1c2d3e4f-5a6b-7c8d-9e0f-1a2b3c4d5002")).Value
        });
    }

    public void SeedUsuarios()
    {
        dbContext.Usuarios.AddRange([
            Usuario.Criar("admin", Email.Criar("admin@example.com").Value, "z74Cnv1c7kbMUoXijDL+y5V/RKXmM3d8J33bdoh9zcs=:8czMbpi+pzqYbZ+mIkB2uwF4NicQeFIDWbjiXVq+Tr6VpGkEcOSwnE1eWs2g+pztieseS9NB1GiPN7woT4GHJw==", Guid.Parse("8b176d2f-fb7f-412b-bdb5-9b82a876b995"))
        ]);
    }

    public void SeedClientes()
    {
        dbContext.Clientes.AddRange([
            Cliente.Criar(CPF.Criar("51363622862").Value, "João", Email.Criar("johndoe@email.com.br").Value, Guid.Parse("31d20d34-b486-4731-ae4e-e82ee174ad3d")),
        ]);
    }

    public void SeedVeiculos()
    {
        dbContext.Veiculos.AddRange([
            Veiculo.Criar(Placa.Criar("ABC-1234").Value, "BYD King", 2025, Guid.Parse("31d20d34-b486-4731-ae4e-e82ee174ad3d"), Guid.Parse("a23f8d8a-5b14-4e13-be17-ec51d77f68f5"))
        ]);
    }
}
