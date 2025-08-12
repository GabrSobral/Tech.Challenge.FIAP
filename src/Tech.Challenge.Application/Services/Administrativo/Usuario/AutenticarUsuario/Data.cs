using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Application.Services.Administrativo.Usuario.AutenticarUsuario;

public record Request(
    Email Email,
    string Password);

public record RequestBody(
    string Email,
    string Password);

public record Response(string AccessToken);
