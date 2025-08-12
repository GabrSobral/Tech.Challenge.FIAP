using Microsoft.Extensions.Logging;
using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Exceptions;
using Tech.Challenge.Domain.Interfaces;
using Tech.Challenge.Domain.Interfaces.Repositories;

namespace Tech.Challenge.Application.Services.Administrativo.Usuario.RegistrarUsuario;

public class RegistrarUsuarioService(
    ILogger<RegistrarUsuarioService> Logger,
    IUsuarioRepository UsuarioRepository,
    IPasswordEncrypter PasswordEncrypter,
    IUnitOfWork UnitOfWork)
{
    public async Task<Result<Response>> Execute(Request request, CancellationToken cancellationToken)
    {
        var usuario = await UsuarioRepository.GetUsuarioByEmail(request.Email, cancellationToken);

        if (usuario is not null)
            return Result.Failure<Response>(new UsuarioAlreadyExistsException("Usuário já cadastrado com este e-mail."));

        var usuarioId = Guid.NewGuid();

        var passwordHash = PasswordEncrypter.Encrypt(request.Password, usuarioId);

        var novoUsuario = Challenge.Domain.Entities.Usuario.Usuario.Criar(request.Nome, request.Email, passwordHash, usuarioId);

        await UsuarioRepository.AdicionarUsuario(novoUsuario, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);

        Logger.LogInformation("Usuário {Email} registrado com sucesso.", request.Email.Endereco);

        return Result.Success(new Response(novoUsuario.Id));
    }
}
