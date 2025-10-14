using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Enums;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico;

public class MonitorarOrdensServicoService(
    ILogger<MonitorarOrdensServicoService> Logger,
    IOrdemServicoRepository OrdemServicoRepository)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("Iniciando o processo de consulta da ordem de serviço.");

        try
        {
            IEnumerable<Challenge.Domain.Entities.OrdemServico.OrdemServico> ordensServico = 
                await OrdemServicoRepository.GetOrdensServico(request.Page ?? 1, request.Take ?? 50, cancellationToken);

            // Remover ordens já deletadas da listagem
            ordensServico = ordensServico.Where(o => o.DeletadoEm is null);

            List<OrdemServicoResponse> ordemServicoResponse = [];

            foreach (var item in ordensServico)
            {
                if (item.Status is EServiceOrderStatus.COMPLETED or EServiceOrderStatus.DELIVERED)
                {
                    continue;
                }
                
                var servicosDb = await OrdemServicoRepository.GetServicosByOrdemServico(item.Id, cancellationToken);
                var produtosDb = await OrdemServicoRepository.GetProdutosByOrdemServico(item.Id, cancellationToken);

                item.Servicos = [.. servicosDb];
                item.Produtos = [.. produtosDb];

                var produtos = item.Produtos.Select(p => new ProdutoResponse(
                    p.Id,
                    p.Produto.Nome,
                    p.Quantidade,
                    p.Produto.Tipo.ToString(),
                    p.PrecoUnitario
                ));

                var servicos = item.Servicos.Select(s => new ServicoResponse(
                    s.Id,
                    s.Servico.Nome,
                    s.PrecoServico
                ));

                var precoTotal = (decimal)(servicosDb.Sum(x => x.PrecoServico) + produtosDb.Sum(x => x.PrecoUnitario * x.Quantidade));

                ordemServicoResponse.Add(new OrdemServicoResponse(
                    item.Id, 
                    item.Status.ToString(), 
                    precoTotal, 
                    item.CriadaEm, 
                    item.EntregueEm, 
                    produtos, 
                    servicos));
            }

            List<string> statusOrder =
            [
                EServiceOrderStatus.IN_PROGRESS.ToString(),
                EServiceOrderStatus.AWAITING_APPROVAL.ToString(),
                EServiceOrderStatus.IN_DIAGNOSIS.ToString(),
                EServiceOrderStatus.RECEIVED.ToString()
            ];

            // Ordenando pela ordem dos status e, em seguida, pelas mais antigas primeiro
            ordemServicoResponse = [.. ordemServicoResponse
                .OrderBy(o => statusOrder.IndexOf(o.Status))
                .ThenBy(o => o.CriadaEm)];

            decimal tempoMedioEntrega = await OrdemServicoRepository.GetTempoMedioEntrega(cancellationToken);

            var totalMinutesInt = (int)Math.Round(tempoMedioEntrega);
            var hours = totalMinutesInt / 60;
            var minutes = totalMinutesInt % 60;

            var tempoEntregaFormatted = $"{hours:D2}h{minutes:D2}";

            return Result.Success(new Response(tempoMedioEntrega, tempoEntregaFormatted, ordemServicoResponse));
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "Erro ao consultar a ordem de serviço.");
            return Result.Failure<Response>(ex);
        }
    }
}
