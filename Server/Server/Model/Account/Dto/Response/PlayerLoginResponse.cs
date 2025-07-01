namespace Server.Model.Account.Dto.Response;

public class PlayerLoginResponse
{
    public string PlayerId { get; set; } = null!;
    public string PlayerName { get; set; } = null!;
    public string SessionId { get; set; } = null!;
}