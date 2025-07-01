using Server.Model.Account.Entity;

namespace Server.Service.Interface;

public interface ISessionService
{
    Task<string> CreateSessionAsync(string accountId);
    Task<PlayerAccountSession?> GetSessionAsync(string sessionId);
    Task<bool> ValidateSessionAsync(string sessionId);
    Task<bool> ExpireSessionAsync(string sessionId);
}