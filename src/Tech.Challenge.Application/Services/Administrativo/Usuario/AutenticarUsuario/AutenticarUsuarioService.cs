using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Usuario.AutenticarUsuario;

public class AutenticarUsuarioService(
    ILogger<AutenticarUsuarioService> Logger,
    IPasswordEncrypter PasswordEncrypter,
    IUsuarioRepository UserRepository,
    IJsonWebToken JsonWebToken)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        Logger.LogInformation("AutenticarUsuarioService - Executando autenticação do usuário com email: {Email}", request.Email);

        var user = await UserRepository.GetUsuarioByEmail(request.Email, cancellationToken);

        if (user is null)
            return Result.Failure<Response>(new UnauthorizedException("E-mail/Senha inválido"));

        if (!PasswordEncrypter.Compare(user.Password, request.Password, user.Id))
            return Result.Failure<Response>(new UnauthorizedException("E-mai/Senha inválido"));

        var accessToken = JsonWebToken.Sign(new JsonWebTokenPayload(user.Id, user.Email.Endereco, user.Name, null));

        Logger.LogInformation($"Autenticação feira com sucesso: {user.Email.Endereco}");

        return Result.Success(new Response(accessToken));
    }
}
