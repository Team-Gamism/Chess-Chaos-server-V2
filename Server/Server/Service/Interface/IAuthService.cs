using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;

namespace Server.Service.Interface;

public interface IAuthService
{
    Task<PlayerLoginResponse> LoginAsync(PlayerLoginRequest req);
    Task<PlayerRegisterResponse> RegisterAsync(PlayerRegisterRequest req);
}