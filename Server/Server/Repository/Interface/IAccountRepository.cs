using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface IAccountRepository
{
    Task<bool> CheckExistsAsync(string playerId, string email);
    Task CreatePlayerAccountAsync(PlayerAccountData account);
    Task<PlayerAccountData?> GetByPlayerIdAsync(string id);
    Task<PlayerAccountData?> GetByEmailAsync(string email);
}