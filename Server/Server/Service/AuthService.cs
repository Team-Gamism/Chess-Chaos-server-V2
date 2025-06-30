using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISessionRepository _sessionRepository;

    public AuthService(IAccountRepository accountRepository, ISessionRepository sessionRepository)
    {
        _accountRepository = accountRepository;
        _sessionRepository = sessionRepository;
    }
    
    public async Task<string> LoginAsync(string playerId, string password)
    {
        var player = await _accountRepository.GetByPlayerIdAsync(playerId);
        if (player == null || !BCrypt.Net.BCrypt.Verify(password, player.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var session = new PlayerAccountSession
        {
            SessionId = Guid.NewGuid().ToString(),
            AccountId = player.Id,
            CreatedAt = DateTime.UtcNow,
            ExpiredAt = DateTime.UtcNow.AddDays(1)
        };
        
        var sessionId = await _sessionRepository.CreateSessionAsync(session);
        
        return sessionId;
    }

    public async Task<bool> RegisterAsync(string playerId, string password, string email)
    {
        bool isExists = await _accountRepository.CheckExistsAsync(playerId, email);
        if (isExists)
            return false;
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

        await _accountRepository.AddPlayerAccountAsync(new PlayerAccountData
        {
            PlayerId = playerId,
            Password = hashedPassword,
            Email = email
        });

        return true;
    }
}