using Dapper;
using MySqlConnector;
using Server.Model.Account.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class SessionRepository : ISessionRepository
{
    private readonly string _connectionString;

    public SessionRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("AccountDb");
    }
    private MySqlConnection CreateConnection() => new(_connectionString);
    
    public async Task<string> CreateSessionAsync(PlayerAccountSession session)
    {
        const string sql = @"
                    INSERT INTO player_account_session (session_id, account_id, created_at, expired_at)
                    VALUES (@SessionId, @AccountId, @CreatedAt, @ExpiredAt);
                    ";

        await using var connection = CreateConnection();
        await connection.OpenAsync();

        await connection.ExecuteAsync(sql, new
        {
            session.SessionId,
            session.AccountId,
            session.CreatedAt,
            session.ExpiredAt
        });

        return session.SessionId;
    }

    public async Task<bool> HasValidSessionAsync(string sessionId)
    {
        const string sql = @"
                    SELECT COUNT(1)
                    FROM player_account_session
                    WHERE session_id = @SessionId
                    AND expired_at > NOW();
                    ";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { SessionId = sessionId });
        return count > 0;
    }
}