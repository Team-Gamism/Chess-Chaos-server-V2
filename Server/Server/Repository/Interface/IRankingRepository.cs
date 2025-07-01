using Server.Model.Data.Entity;

namespace Server.Repository.Interface;

public interface IRankingRepository
{
    Task<IEnumerable<PlayerRankingData>> GetAllRankingDataAsync();
    Task<PlayerRankingData> GetRankingDataAsync(string playerId);
    Task AddPlayerRankingAsync(PlayerRankingData data);
    Task UpdatePlayerRankingAsync(string playerId, int newScore);
}