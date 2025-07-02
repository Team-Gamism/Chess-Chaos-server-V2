using Server.Model.Data.Entity;

namespace Server.Model.Data.Dto.Response;

public class AllPlayerRankingResponse
{
    public int Count { get; set; }
    public List<PlayerRankingData> Rankings { get; set; } = new();
}