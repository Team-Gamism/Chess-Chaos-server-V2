namespace Server.Service.Interface;

public interface IAuthService
{
    Task<string> LoginAsync(string playerId, string password);
    Task<bool> RegisterAsync(string playerId, string password, string email);
}