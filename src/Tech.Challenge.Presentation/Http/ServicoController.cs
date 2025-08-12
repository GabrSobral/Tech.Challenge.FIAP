using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Application.Services.Administrativo.Servicos.AdicionarServico;
using Tech.Challenge.Application.Services.Administrativo.Servicos.AtualizarServico;
using Tech.Challenge.Application.Services.Administrativo.Servicos.DeletarServico;
using Tech.Challenge.Application.Services.Administrativo.Servicos.ListarServicos;

namespace Tech.Challenge.Presentation.Http;

[Authorize]
[Route("servicos")]
[ApiController]
public class ServicoController(
    IHttpContextAccessor HttpContextAcessor,
    AdicionarServicoService AdicionarServicoService,
    DeletarServicoService DeletarServicoService,
    AtualizarServicoService AtualizarServicoService,
    ListarServicosService ListarServicosService) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListarServicos(
        [FromQuery] int? page,
        [FromQuery] int? take,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Servicos.ListarServicos.Request(page, take);

            var response = await ListarServicosService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost()]
    public async Task<IActionResult> AdicionarServicos(
        [FromBody] Application.Services.Administrativo.Servicos.AdicionarServico.Request body,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await AdicionarServicoService.Execute(body, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("{servicoId}")]
    public async Task<IActionResult> DeletarServico(
        [FromRoute] Guid servicoId,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Servicos.DeletarServico.Request(servicoId);
            var response = await DeletarServicoService.Execute(request, cancellationToken);

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
    public async Task<IActionResult> AtualizarServico(
        [FromRoute] Guid id,
        [FromBody] Application.Services.Administrativo.Servicos.AtualizarServico.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Servicos.AtualizarServico.Request(id, body.Nome, body.PrecoServico);

            var response = await AtualizarServicoService.Execute(request, cancellationToken);

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
