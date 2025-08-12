using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Application.Services.Administrativo.Cliente.AtualizarCliente;
using Tech.Challenge.Application.Services.Administrativo.Cliente.CadastrarCliente;
using Tech.Challenge.Application.Services.Administrativo.Cliente.DeletarCliente;
using Tech.Challenge.Application.Services.Administrativo.Cliente.IdentificarCliente;

namespace Tech.Challenge.Presentation.Http;

[Authorize]
[ApiController]
[Route("clientes")]
public class ClienteController(
    IHttpContextAccessor HttpContextAccessor,
    CadastrarClienteService CadastrarClienteService,
    IdentificarClienteService IdentificarClienteService,
    DeletarClienteService DeletarClienteService,
    AtualizarClienteService AtualizarClienteService
) : ControllerBase
{
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleterCliente([FromRoute] Guid id, CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Cliente.DeletarCliente.Request(id);

            var response = await DeletarClienteService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return NoContent();
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("{cpf}")]
    public async Task<IActionResult> IdentificarCliente([FromRoute] string cpf, CancellationToken cancellationToken)
    {
        try
        {
            var request = new Application.Services.Administrativo.Cliente.IdentificarCliente.Request(cpf);

            var response = await IdentificarClienteService.Execute(request, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("")]
    public async Task<IActionResult> CadastrarCliente(
        [FromBody] Application.Services.Administrativo.Cliente.CadastrarCliente.Request body, 
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await CadastrarClienteService.Execute(body, cancellationToken);

            if (response.IsFailure)
                throw response.Error!;

            return Ok(response.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> AtualizarCliente(
        [FromRoute] Guid id,
        [FromBody] Application.Services.Administrativo.Cliente.AtualizarCliente.RequestBody body,
        CancellationToken cancellationToken)
    {
        try
        {
            var cpf = CPF.Criar(body.Cpf);

            if (cpf.IsFailure)
                throw cpf.Error!;

            var email = Email.Criar(body.Email);

            if (email.IsFailure)
                throw email.Error!;

            var request = new Application.Services.Administrativo.Cliente.AtualizarCliente.Request(id, cpf.Value, body.Nome, email.Value);

            var response = await AtualizarClienteService.Execute(request, cancellationToken);

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
