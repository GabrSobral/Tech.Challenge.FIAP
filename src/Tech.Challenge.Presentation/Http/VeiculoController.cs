using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Domain.Entities.Veiculo.ValueObjects;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.AtualizarVeiculo;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.CadastrarVeiculo;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.ConsultarVeiculoPelaPlaca;
using Tech.Challenge.Application.Services.Administrativo.Veiculo.DeletarVeiculo;

namespace Tech.Challenge.Presentation.Http;

[Authorize]
[Route("veiculos")]
[ApiController]
public class VeiculoController(
    IHttpContextAccessor HttpContextAccessor,
    CadastrarVeiculoService CadastrarVeiculoService,
    ConsultarVeiculoPelaPlacaService ConsultarVeiculoPelaPlacaService,
    DeletarVeiculoService DeletarVeiculoService,
    AtualizarVeiculoService AtualizarVeiculoService
) : ControllerBase
{
    [HttpPost("")]
    public async Task<IActionResult> CadastrarCliente(
        [FromBody] Application.Services.Administrativo.Veiculo.CadastrarVeiculo.Request body,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await CadastrarVeiculoService.Execute(body, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{placa}")]
    public async Task<IActionResult> GetClienteByCpf([FromRoute] string placa, CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Veiculo.ConsultarVeiculoPelaPlaca.Request(placa);

            var response = await ConsultarVeiculoPelaPlacaService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletarVeiculo([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Veiculo.DeletarVeiculo.Request(id);

            var response = await DeletarVeiculoService.Execute(request, cancellationToken);

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
    public async Task<IActionResult> AtualizarVeiculo(
        [FromRoute] Guid id,
        [FromBody] Application.Services.Administrativo.Veiculo.AtualizarVeiculo.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var placa = Placa.Criar(body.Placa);

            if (placa.IsFailure)
                throw placa.Error!;

            var request = new Application.Services.Administrativo.Veiculo.AtualizarVeiculo.Request(id, placa.Value, body.Modelo, body.Ano);

            var response = await AtualizarVeiculoService.Execute(request, cancellationToken);

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
