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
                    insert into player_account_session (session_id, account_id, created_at, expired_at)
                    values (@SessionId, @AccountId, @CreatedAt, @ExpiredAt);
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
                    select COUNT(1)
                    from player_account_session
                    where session_id = @SessionId
                    and expired_at > NOW();
                    ";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { SessionId = sessionId });
        return count > 0;
    }

    public async Task<PlayerAccountSession?> GetSessionAsync(string sessionId)
    {
        const string sql = @"
                    select session_id AS SessionId, account_id AS AccountId, created_at AS CreatedAt, expired_at AS ExpiredAt
                    from player_account_session
                    where session_id = @SessionId;
                    ";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        return await connection.QueryFirstOrDefaultAsync<PlayerAccountSession>(sql, new { SessionId = sessionId });
    }

    public async Task<bool> ExpireSessionAsync(string sessionId)
    {
        const string sql = @"
                    delete from player_account_session 
                    where session_id = @SessionId;";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var rows = await connection.ExecuteAsync(sql, new { SessionId = sessionId });
        return rows > 0;
    }
}