using Dapper;
using MySqlConnector;
using Server.Model.Account.Entity;
using Server.Repository.Interface;

namespace Server.Repository;

public class AccountRepository : IAccountRepository
{
    private readonly string _connectionString;

    public AccountRepository(IConfiguration config)
    {
        _connectionString = config.GetConnectionString("AccountDb");
    }
    private MySqlConnection CreateConnection() => new(_connectionString);

    public async Task<bool> CheckExistsAsync(string playerId)
    {
        const string sql = @"
                    select count(1)
                    from player_account_data
                    where player_id = @PlayerId;
                    ";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId });
        
        return count > 0;
    }

    public async Task AddPlayerAccountAsync(PlayerAccountData account)
    {
        const string sql = @"
                    insert into player_account_data
                    (id, player_id, player_name, password, email, created_at) 
                    values
                    (@Id, @PlayerId, @PlayerName, @Password, @Email, @CreatedAt);
                    ";

        await using var connection = CreateConnection();
        await connection.OpenAsync();
        await connection.ExecuteAsync(sql, account);
    }

    public async Task<PlayerAccountData?> GetByPlayerIdAsync(string id)
    {
        const string sql = @"
                    select
                        id,
                        player_id as PlayerId,
                        player_name as PlayerName,
                        password as Password,
                        email as Email,
                        created_at AS CreatedAt,
                        last_login_at AS LastLoginAt
                    from player_account_data
                    where player_id = @Id
                    limit 1";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<PlayerAccountData>(sql, new {Id = id});
    }

    public async Task<PlayerAccountData?> GetByEmailAsync(string email)
    {
        const string sql = @"
                    select
                        id,
                        player_id as PlayerId,
                        player_name as PlayerName,
                        password as Password,
                        email as Email,
                        created_at AS CreatedAt,
                        last_login_at AS LastLoginAt
                    from player_account_data
                    where email = @Email
                    limit 1";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<PlayerAccountData>(sql, new {Email = email});
    }

    public async Task UpdatePlayerAsync(PlayerAccountData player)
    {
        const string sql = @"
                    update player_account_data
                    set player_name = @PlayerName,
                        password = @Password,
                        email = @Email,
                        last_login_at = @LastLoginAt
                    where id = @Id";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        await connection.ExecuteAsync(sql, player);
    }
}