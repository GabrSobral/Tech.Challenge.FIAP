using Microsoft.EntityFrameworkCore;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;
using Tech.Challenge.Domain.Entities.Usuario;
using Tech.Challenge.Domain.Interfaces.Repositories;
using Tech.Challenge.Infra.Database.Contexts;

namespace Tech.Challenge.Infra.Database.Repositories;

public class UsuarioRepository(DataContext dbContext) : IUsuarioRepository
{
    public async Task AdicionarUsuario(Usuario usuario, CancellationToken cancellationToken)
    {
        await dbContext.Usuarios.AddAsync(usuario, cancellationToken);
    }

    public async Task<Usuario?> GetUsuarioByEmail(Email email, CancellationToken cancellationToken)
    {
        return await dbContext.Usuarios
            .FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
    }

    public async Task<Usuario?> GetUsuarioById(Guid usuarioId, CancellationToken cancellationToken)
    {
        return await dbContext.Usuarios
            .FirstOrDefaultAsync(u => u.Id == usuarioId, cancellationToken);
    }
}
