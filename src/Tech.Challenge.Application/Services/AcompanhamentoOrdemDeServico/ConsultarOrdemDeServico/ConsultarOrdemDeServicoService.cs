using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.OrdemServico;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ConsultarOrdemDeServico;

public class ConsultarOrdemDeServicoService(
    ILogger<ConsultarOrdemDeServicoService> logger,
    IOrdemServicoRepository ordemServicoRepository,
    IClienteRepository clienteRepository)
{
    public async Task<Result<IEnumerable<Response>>> Execute(Request request, CancellationToken cancellationToken)
    {
        logger.LogInformation("Iniciando o processo de consulta da ordem de serviço.");

        var cpf = CPF.Criar(request.Cpf);

        if (cpf.IsFailure)
        {
            logger.LogWarning("CPF inválido: {Cpf}", request.Cpf);
            return Result.Failure<IEnumerable<Response>>(cpf.Error!);
        }

        try
        {
            var cliente = await clienteRepository.GetClienteByCpf(cpf.Value, cancellationToken);

            if (cliente is null)
            {
                logger.LogWarning($"Cliente com CPF {cpf.Value} não encontrado.");
                return Result.Failure<IEnumerable<Response>>(new ClienteNotFoundException(cpf.Value));
            }

            IEnumerable<OrdemServico> ordemServico = [];

            if (request.ordemServicoId is null)
            {
                ordemServico = await ordemServicoRepository.GetOrdensServicoByCpf(cpf.Value, cancellationToken);
            } else
            {
                Guid ordemServicoId = (Guid)request.ordemServicoId;

                var ordemServicoDb = await ordemServicoRepository.GetOrdemServicoById(ordemServicoId, cancellationToken);

                if (ordemServicoDb is null)
                {
                    return Result.Failure<IEnumerable<Response>> (new OrdemServicoNotFoundException(ordemServicoId));
                }

                ordemServico = [ordemServicoDb];
            }

            List<Response> response = [];

            foreach (var item in ordemServico)
            {
                var servicosDb = await ordemServicoRepository.GetServicosByOrdemServico(item.Id, cancellationToken);
                var produtosDb = await ordemServicoRepository.GetProdutosByOrdemServico(item.Id, cancellationToken);

                item.Servicos = [.. servicosDb];
                item.Produtos = [.. produtosDb];

                var produtos = item.Produtos.Select(p => new ProdutoResponse(
                    p.Id,
                    p.Produto.Nome,
                    p.Quantidade,
                    nameof(p.Produto.Tipo),
                    p.PrecoUnitario
                ));

                var servicos = item.Servicos.Select(s => new ServicoResponse(
                    s.Id,
                    s.Servico.Nome,
                    s.PrecoServico
                ));

                var precoTotal = (decimal)(servicosDb.Sum(x => x.PrecoServico) + produtosDb.Sum(x => x.PrecoUnitario * x.Quantidade));

                response.Add(new Response(item.Id, item.Status.ToString(), item.CriadaEm, precoTotal, produtos, servicos));
            }

            response.Sort((a, b) => b.CriadaEm.CompareTo(a.CriadaEm));

            return Result.Success<IEnumerable<Response>>(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Erro ao consultar a ordem de serviço.");
            return Result.Failure<IEnumerable<Response>>(ex);
        }
    }
}
