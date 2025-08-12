using System.ComponentModel.DataAnnotations.Schema;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;

namespace Tech.Challenge.Domain.Entities.Veiculo;

public class Veiculo : Entity
{
    public required Placa Placa { get; set; }

    public string Modelo { get; set; } = string.Empty;

    public Guid ClienteId { get; set; }

    public required uint Ano { get; set; }

    public DateTime AdicionadoEm { get; set; }

    public DateTime AtualizadoEm { get; set; }

    [ForeignKey(nameof(ClienteId))]
    public Cliente.Cliente Cliente { get; set; }

    public static Veiculo Criar(Placa placa, string modelo, uint ano, Guid clienteId)
    {
        return new Veiculo()
        {
            Id = Guid.NewGuid(),
            Placa = placa,
            Modelo = modelo,
            Ano = ano,
            ClienteId = clienteId,
            AdicionadoEm = DateTime.UtcNow
        };
    }

    public static Veiculo Criar(Placa placa, string modelo, uint ano, Guid clienteId, Guid id)
    {
        return new Veiculo()
        {
            Id = id,
            Placa = placa,
            Modelo = modelo,
            Ano = ano,
            ClienteId = clienteId,
            AdicionadoEm = DateTime.UtcNow
        };
    }

    public void Atualizar(Placa placa, string modelo, uint ano)
    {
        Placa = placa;
        Modelo = modelo;
        Ano = ano;
        AtualizadoEm = DateTime.UtcNow;
    }
}
