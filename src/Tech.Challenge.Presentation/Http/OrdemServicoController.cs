using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ConsultarOrdemDeServico;
using Tech.Challenge.Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.DeletarOrdemDeServico;
using Tech.Challenge.Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico;

namespace Tech.Challenge.Presentation.Http;

[Authorize]
[Route("api/ordem-servico")]
[ApiController]
public class OrdemServicoController(
    IHttpContextAccessor HttpContextAccessor,
    CriarOrdemDeServicoService CriarOrdemDeServicoService,
    ConsultarOrdemDeServicoService ConsultarOrdemDeServicoService,
    AtualizarStatusOrdemDeServico AtualizarStatusOrdemDeServicoService,
    AprovarOrdemDeServicoService AprovarOrdemDeServicoService,
    AtualizarOrdemDeServicoService AtualizarORdemDeServicoService,
    DeletarOrdemDeServicoService DeletarOrdemDeServicoService,
    ReprovarOrdemDeServicoService ReprovarOrdemDeServicoService,
    MonitorarOrdensServicoService MonitorarOrdensServicoService
    ) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CriarOrdemDeServico(
        [FromBody] Application.Services.Administrativo.OrdemServico.CriarOrdemDeServico.Request body,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await CriarOrdemDeServicoService.Execute(body, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [AllowAnonymous]
    [HttpGet("cliente/{cpf}")]
    public async Task<IActionResult> ConsultarOrdemDeServico(
        [FromRoute] string cpf, 
        [FromQuery] Guid? ordemServicoId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.AcompanhamentoOrdemDeServico.ConsultarOrdemDeServico.Request(cpf, ordemServicoId);

            var response = await ConsultarOrdemDeServicoService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("monitoramento")]
    public async Task<IActionResult> MonitorarOrdensServico(
    [FromQuery] int? page,
    [FromQuery] int? take,
    CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.OrdemServico.MonitorarOrdensServico.Request(page, take);

            var response = await MonitorarOrdensServicoService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpPatch("status/{ordemServicoId}")]
    public async Task<IActionResult> AtualizarStatusOrdemDeServico(
        [FromRoute] Guid ordemServicoId,
        [FromBody] Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await AtualizarStatusOrdemDeServicoService.Execute(
                new Application.Services.Administrativo.OrdemServico.AtualizarStatusOrdemDeServico.Request(ordemServicoId, body.Status),
                cancellationToken
            );

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPatch("aprovar/{ordemServicoId}")]
    public async Task<IActionResult> AprovarOrdemDeServico(
        [FromRoute] Guid ordemServicoId,
        [FromBody] Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var clienteCpf = CPF.Criar(body.ClienteCpf);

            if (clienteCpf.IsFailure)
                throw clienteCpf.Error!;

            var response = await AprovarOrdemDeServicoService.Execute(
                new Application.Services.AcompanhamentoOrdemDeServico.AprovarOrdemDeServico.Request(clienteCpf.Value, ordemServicoId),
                cancellationToken
            );

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [AllowAnonymous]
    [HttpPatch("reprovar/{ordemServicoId}")]
    public async Task<IActionResult> ReprovarOrdemDeServico(
    [FromRoute] Guid ordemServicoId,
    [FromBody] Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico.RequestBody body,
    CancellationToken cancellationToken)
    {
        try
        {
            var clienteCpf = CPF.Criar(body.ClienteCpf);

            if (clienteCpf.IsFailure)
                throw clienteCpf.Error!;

            var response = await ReprovarOrdemDeServicoService.Execute(
                new Application.Services.AcompanhamentoOrdemDeServico.ReprovarOrdemDeServico.Request(clienteCpf.Value, ordemServicoId),
                cancellationToken
            );

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarOrdemDeServico(
        [FromRoute] Guid id,
        [FromBody] Application.Services.Administrativo.OrdemServico.AtualizarOrdemDeServico.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.OrdemServico.AtualizarOrdemDeServico.Request(id, body.Servicos, body.Produtos);

            var response = await AtualizarORdemDeServicoService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarOrdemDeServico(
        [FromRoute] Guid id,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.OrdemServico.DeletarOrdemDeServico.Request(id);

            var response = await DeletarOrdemDeServicoService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }
}
