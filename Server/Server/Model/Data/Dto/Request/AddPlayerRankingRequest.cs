namespace Server.Model.Data.Dto.Request;

public class AddPlayerRankingRequest
{
    public string PlayerId { get; set; } = null!;
    public int PlayerScore { get; set; }
}