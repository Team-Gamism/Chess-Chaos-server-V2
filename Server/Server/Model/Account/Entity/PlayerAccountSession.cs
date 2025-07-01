namespace Server.Model.Account.Entity;

public class PlayerAccountSession
{
    public string SessionId { get; set; } = null!;
    public string AccountId { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiredAt { get; set; }
}