using Dapper;
using MySqlConnector;
using Server.Model.Data.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class RankingRepository :  IRankingRepository
{
    private readonly string _connectionString;

    public RankingRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("GameDataDb");
    }

    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task<IEnumerable<PlayerRankingData>> GetAllRankingDataAsync()
    {
        const string sql = @"
                    select ranking as Ranking,
                        player_id as PlayerId,
                        player_score as PlayerScore
                    from player_ranking_data;";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();

        var result = await connection.QueryAsync<PlayerRankingData>(sql);
        return result;
    }

    public async Task<PlayerRankingData> GetRankingDataAsync(string playerId)
    {
        const string sql = @"
                    select ranking as Ranking,
                        player_id as PlayerId,
                        player_score as PlayerScore
                    from player_ranking_data
                    where player_id = @player_id;";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var result = await connection.QueryFirstOrDefaultAsync<PlayerRankingData>(sql, new { player_id = playerId });
        return result;
    }

    public async Task AddPlayerRankingAsync(PlayerRankingData data)
    {
        const string sql = @"
                    insert into player_ranking_data
                    (ranking, player_id, player_score)
                    values (@ranking, @player_id, @player_score);";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, data);
    }

    public async Task UpdatePlayerRankingAsync(string playerId, int newScore)
    {
        const string sql = @"
                    update player_ranking_data
                    set player_score = @newScore
                    where player_id = @playerId;";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, new { playerId, newScore });
    }
}