namespace Tech.Challenge.Domain.Interfaces;

public interface IPasswordEncrypter
{
    public string Encrypt<T>(string rawPassword, T? userId);
    public bool Compare<T>(string storedPassword, string enteredPassword, T? userId);
}
