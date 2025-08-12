using Tech.Challenge.Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico;

namespace Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarOrdemDeServico;

public record Request(
    Guid OrdemServicoId,
    IEnumerable<Guid> Servicos,
    IEnumerable<ProdutosRequest> Produtos);

public record RequestBody(
    IEnumerable<Guid> Servicos,
    IEnumerable<ProdutosRequest> Produtos);
