using Server.Model.Account.Entity;

namespace Server.Repository.Interface;

public interface IAccountRepository
{
    Task<bool> CheckExistsAsync(string playerId);
    Task AddPlayerAccountAsync(PlayerAccountData account);
    Task<PlayerAccountData?> GetByPlayerIdAsync(string id);
    Task<PlayerAccountData?> GetByEmailAsync(string email);
    Task UpdatePlayerAsync(PlayerAccountData player);
}