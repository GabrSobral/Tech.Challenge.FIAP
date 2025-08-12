using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.Usuario;

namespace Tech.Challenge.Domain.Interfaces.Repositories;

public interface IUsuarioRepository
{
    public Task AdicionarUsuario(Usuario usuario, CancellationToken cancellationToken);

    public Task<Usuario?> GetUsuarioById(Guid usuarioId, CancellationToken cancellationToken);

    public Task<Usuario?> GetUsuarioByEmail(Email email, CancellationToken cancellationToken);
}
