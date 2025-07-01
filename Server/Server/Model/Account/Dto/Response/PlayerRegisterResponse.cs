namespace Server.Model.Account.Dto.Response;

public class PlayerRegisterResponse
{
    public string Id { get; set; } = string.Empty;
    public string PlayerId { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PlayerName { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}