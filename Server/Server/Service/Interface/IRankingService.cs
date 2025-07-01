using Server.Model.Data.Entity;

namespace Server.Service.Interface;

public interface IRankingService
{
    Task<IEnumerable<PlayerRankingData>> GetAllRankingsAsync();
    Task<PlayerRankingData?> GetRankingByPlayerIdAsync(string playerId);
    Task AddPlayerRankingAsync(PlayerRankingData data);
    Task UpdatePlayerScoreAsync(string playerId, int newScore);
}