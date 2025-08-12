using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Application.Services.Administrativo.Usuario.AutenticarUsuario;
using Tech.Challenge.Application.Services.Administrativo.Usuario.RegistrarUsuario;

namespace Tech.Challenge.Presentation.Http;

[Route("auth")]
[ApiController]
public class AuthController(
    IHttpContextAccessor HttpContextAcessor,
    AutenticarUsuarioService AutenticarUsuarioService,
    RegistrarUsuarioService RegistrarUsuarioService) : ControllerBase
{
    [HttpPost("login")]
    public async Task<IActionResult> AutenticarUsuario(
        [FromBody] Application.Services.Administrativo.Usuario.AutenticarUsuario.RequestBody requestBody, 
        CancellationToken cancellationToken)
    {
        try
        {
            var email = Email.Criar(requestBody.Email);

            if (email.IsFailure)
                throw email.Error!;

            var request = new Application.Services.Administrativo.Usuario.AutenticarUsuario.Request(
                email.Value,
                requestBody.Password);

            var result = await AutenticarUsuarioService.Execute(request, cancellationToken);

            if (result.IsFailure)
                throw result.Error!;

            return Ok(result.Value);
        }
        catch
        {
            throw;
        }
    }

    [HttpPost("registrar")]
    public async Task<IActionResult> RegistrarUsuario([FromBody] 
        Application.Services.Administrativo.Usuario.RegistrarUsuario.RequestBody requestBody, 
        CancellationToken cancellationToken)
    {
        try
        {
            var email = Email.Criar(requestBody.Email);

            if (email.IsFailure)
                throw email.Error!;

            var request = new Application.Services.Administrativo.Usuario.RegistrarUsuario.Request(
                requestBody.Nome,
                email.Value,
                requestBody.Password);

            var result = await RegistrarUsuarioService.Execute(request, cancellationToken);

            if (result.IsFailure)
                throw result.Error!;

            return Ok(result.Value);
        }
        catch
        {
            throw;
        }
    }
}
