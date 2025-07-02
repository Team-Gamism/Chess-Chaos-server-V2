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
        await _sessionRepository.ExpireSessionAsync(accountId);
        
        var kstNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, 
            TimeZoneInfo.FindSystemTimeZoneById("Asia/Seoul"));

        var session = new PlayerAccountSession
        {
            AccountId = accountId,
            SessionId = Guid.NewGuid().ToString(),
            CreatedAt = kstNow,
            ExpiredAt = kstNow.AddHours(1)
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