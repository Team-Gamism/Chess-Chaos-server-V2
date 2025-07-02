using Server.Model.Data.Entity;
using Server.Repository.Interface;
using Server.Service.Interface;

namespace Server.Service;

public class RankingService : IRankingService
{
    private readonly IRankingRepository  _rankingRepository;
    private readonly IAccountRepository _accountRepository;

    public RankingService(IRankingRepository rankingRepository, IAccountRepository accountRepository)
    {
        _rankingRepository = rankingRepository;
        _accountRepository = accountRepository;
    }
    
    public async Task<IEnumerable<PlayerRankingData>> GetAllRankingsAsync()
    {
        return await _rankingRepository.GetAllRankingDataAsync();
    }

    public async Task<PlayerRankingData?> GetRankingByPlayerIdAsync(string playerId)
    {
        return await _rankingRepository.GetRankingDataAsync(playerId);
    }

    public async Task AddPlayerRankingAsync(PlayerRankingData data)
    {
        if (!await _accountRepository.CheckExistsAsync(data.PlayerId))
            throw new Exception("Player does not exist");
        
        var existingRanking = await _rankingRepository.GetRankingDataAsync(data.PlayerId);
        if (existingRanking == null)
        {
            data.Ranking = 0;
            await _rankingRepository.AddPlayerRankingAsync(data);
        }
        else
        {
            await _rankingRepository.UpdatePlayerRankingByNewScoreAsync(data.PlayerId, data.PlayerScore);
        }
        
        await CalculateRankingsAsync();
    }

    public async Task UpdatePlayerScoreAsync(string playerId, int newScore)
    {
        await _rankingRepository.UpdatePlayerRankingByNewScoreAsync(playerId, newScore);
    }
    
    private async Task CalculateRankingsAsync()
    {
        var allRankings = (await _rankingRepository.GetAllRankingDataAsync())
            .OrderByDescending(r => r.PlayerScore)
            .ToList();

        for (int i = 0; i < allRankings.Count; i++)
        {
            var rankingData = allRankings[i];
            int newRank = i + 1;
            if (rankingData.Ranking != newRank)
            {
                rankingData.Ranking = newRank;
                await _rankingRepository.UpdatePlayerRankingByNewRankAsync(rankingData.PlayerId, newRank);
            }
        }
    }
}