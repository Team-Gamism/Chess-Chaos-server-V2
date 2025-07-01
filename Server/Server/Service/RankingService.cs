using Server.Model.Data.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class RankingService : IRankingService
{
    private readonly IRankingRepository  _repository;

    public RankingService(IRankingRepository repository)
    {
        _repository = repository;
    }
    
    public async Task<IEnumerable<PlayerRankingData>> GetAllRankingsAsync()
    {
        return await _repository.GetAllRankingDataAsync();
    }

    public async Task<PlayerRankingData?> GetRankingByPlayerIdAsync(string playerId)
    {
        return await _repository.GetRankingDataAsync(playerId);
    }

    public async Task AddPlayerRankingAsync(PlayerRankingData data)
    {
        await _repository.AddPlayerRankingAsync(data);
    }

    public async Task UpdatePlayerScoreAsync(string playerId, int newScore)
    {
        await _repository.UpdatePlayerRankingAsync(playerId, newScore);
    }
}