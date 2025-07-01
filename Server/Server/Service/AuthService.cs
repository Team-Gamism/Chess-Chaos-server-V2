using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;
using Server.Model.Account.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class AuthService : IAuthService
{
    private readonly IAccountRepository _accountRepository;
    private readonly ISessionService _sessionService;

    public AuthService(IAccountRepository accountRepository, ISessionService sessionService)
    {
        _accountRepository = accountRepository;
        _sessionService = sessionService;
    }
    
    public async Task<PlayerLoginResponse> LoginAsync(PlayerLoginRequest req)
    {
        var player = await _accountRepository.GetByPlayerIdAsync(req.PlayerId);
        if (player == null || !BCrypt.Net.BCrypt.Verify(req.Password, player.Password))
            throw new UnauthorizedAccessException("Invalid credentials");

        var sessionId = await _sessionService.CreateSessionAsync(player.Id);
        
        return new PlayerLoginResponse
        {
            PlayerId = player.PlayerId,
            PlayerName = player.PlayerName,
            SessionId = sessionId
        };
    }

    public async Task<PlayerRegisterResponse> RegisterAsync(PlayerRegisterRequest req)
    {
        bool isExists = await _accountRepository.CheckExistsAsync(req.PlayerId, req.Email);
        if (isExists)
            throw new Exception("Player already exists");
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);

        var newPlayer = new PlayerAccountData
        {
            PlayerId = req.PlayerId,
            PlayerName = req.PlayerName,
            Email = req.Email,
            CreatedAt = DateTime.UtcNow,
        };
        
        await _accountRepository.AddPlayerAccountAsync(newPlayer);

        return new PlayerRegisterResponse
        {
            Id = newPlayer.Id,
            PlayerId = newPlayer.PlayerId,
            Email = newPlayer.Email,
            PlayerName = newPlayer.PlayerName,
            CreatedAt = newPlayer.CreatedAt
        };
    }
}