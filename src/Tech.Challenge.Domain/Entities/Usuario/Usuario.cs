using Tech.Challenge.Domain.Core;
using Tech.Challenge.Domain.Entities.Cliente.ValueObjects;

namespace Tech.Challenge.Domain.Entities.Usuario;

public class Usuario : Entity
{
    public string Name { get; set; } = string.Empty;

    public Email Email { get; set; }

    public string Password { get; set; } = string.Empty;

    public static Usuario Criar(string name, Email email, string password, Guid id)
    {
        return new Usuario
        {
            Id = id,
            Name = name,
            Email = email,
            Password = password
        };
    }

    public static Usuario Criar(string name, Email email, string password)
    {
        return new Usuario
        {
            Id = Guid.NewGuid(),
            Name = name,
            Email = email,
            Password = password
        };
    }

    public void Atualizar(string name, Email email, string password)
    {
        Name = name;
        Email = email;
        Password = password;
    }
}
