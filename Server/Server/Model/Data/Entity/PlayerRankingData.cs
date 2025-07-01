namespace Server.Model.Data.Entity;

public class PlayerRankingData
{
    public int Ranking { get; set; }
    public string PlayerId { get; set; } = null!;
    public int PlayerScore { get; set; }
}