using Server.Model.Data.Entity;

namespace Server.Repository.Interface;

public interface IRankingRepository
{
    Task<IEnumerable<PlayerRankingData>> GetAllRankingDataAsync();
    Task<PlayerRankingData?> GetRankingDataAsync(string playerId);
    Task AddPlayerRankingAsync(PlayerRankingData data);
    Task UpdatePlayerRankingByNewScoreAsync(string playerId, int newScore);
    Task UpdatePlayerRankingByNewRankAsync(string playerId, int newRanking);
}