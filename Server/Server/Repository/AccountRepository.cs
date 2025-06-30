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

    public async Task<bool> CheckExistsAsync(string playerId, string email)
    {
        const string sql = @"
                    select count(1)
                    from player_account_data
                    where player_id = @PlayerId or email = @Email;
                    ";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        
        var count = await connection.ExecuteScalarAsync<int>(sql, new { PlayerId = playerId, Email = email });
        
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
                    select * 
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
                    select *
                    from player_account_data
                    where email = @Email
                    limit 1";
        
        await using var connection = CreateConnection();
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<PlayerAccountData>(sql, new {Email = email});
    }
}