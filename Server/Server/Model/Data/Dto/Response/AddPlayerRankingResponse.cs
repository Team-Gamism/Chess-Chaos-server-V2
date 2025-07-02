namespace Server.Model.Data.Dto.Response;

public class AddPlayerRankingResponse
{
    public string PlayerId { get; set; } = null!;
    public int PlayerScore { get; set; }
    public int Ranking { get; set; }
}