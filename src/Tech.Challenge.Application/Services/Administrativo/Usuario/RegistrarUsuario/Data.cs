using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Application.Services.Administrativo.Usuario.RegistrarUsuario;

public record Request(
    string Nome,
    Email Email,
    string Password);

public record RequestBody(
    string Nome,
    string Email,
    string Password);

public record Response(Guid Id);
