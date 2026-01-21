using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Application.Services.Administrativo.Usuario.AutenticarUsuario;
using Tech.Challenge.Application.Services.Administrativo.Usuario.RegistrarUsuario;

namespace Tech.Challenge.Presentation.Http;

[ApiController]
public class HealthController(IHttpContextAccessor HttpContextAcessor) : ControllerBase
{
    [HttpGet("api/health")]
    public async Task<IActionResult> Health1(CancellationToken cancellationToken)
    {
        try
        {
            return Ok("Service is healthy");
        }
        catch
        {
            throw;
        }
    }

    [HttpGet("health")]
    public async Task<IActionResult> Health2(CancellationToken cancellationToken)
    {
        try
        {
            return Ok("Service is healthy");
        }
        catch
        {
            throw;
        }
    }
}
