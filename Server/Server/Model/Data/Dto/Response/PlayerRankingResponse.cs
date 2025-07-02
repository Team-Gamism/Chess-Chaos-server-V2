namespace Server.Model.Data.Dto.Response;

public class PlayerRankingResponse
{
    public string PlayerId { get; set; } = null!;
    public int Ranking { get; set; }
    public int PlayerScore { get; set; }
}