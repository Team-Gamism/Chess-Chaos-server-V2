namespace Server.Model.Account.Dto.Response;

public class PlayerRegisterResponse
{
    public string Id { get; set; } = null!;
    public string PlayerId { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PlayerName { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
}