using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class SessionService : ISessionService
{
    private readonly ISessionRepository _sessionRepository;

    public SessionService(ISessionRepository sessionRepository)
    {
        _sessionRepository = sessionRepository;
    }
    
    public async Task<string> CreateSessionAsync(string accountId)
    {
        var session = new PlayerAccountSession
        {
            AccountId = accountId,
            SessionId = Guid.NewGuid().ToString(),
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddHours(1)
        };
        
        return await _sessionRepository.CreateSessionAsync(session);
    }

    public async Task<PlayerAccountSession?> GetSessionAsync(string sessionId)
    {
        return await _sessionRepository.GetSessionAsync(sessionId);
    }

    public async Task<bool> ValidateSessionAsync(string sessionId)
    {
        return await _sessionRepository.HasValidSessionAsync(sessionId);
    }

    public async Task<bool> ExpireSessionAsync(string sessionId)
    {
        return await _sessionRepository.ExpireSessionAsync(sessionId);
    }
}