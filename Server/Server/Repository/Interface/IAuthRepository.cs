using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface IAuthRepository
{
    Task CreatePlayerAccountAsync(PlayerAccountData account);
    Task<PlayerAccountData?> GetByPlayerIdAsync(string id);
    Task<PlayerAccountData?> GetByEmailAsync(string email);
}