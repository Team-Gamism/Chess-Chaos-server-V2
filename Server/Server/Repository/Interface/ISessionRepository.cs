using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface ISessionRepository
{
    Task<string> CreateSessionAsync(PlayerAccountSession session);
    Task<bool> HasValidSessionAsync(string sessionId);
}