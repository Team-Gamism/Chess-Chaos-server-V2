using Microsoft.AspNetCore.Mvc;
using Server.Model.Data.Dto.Request;
using Server.Model.Data.Dto.Response;
using Server.Model.Data.Entity;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class RankingController : ControllerBase
    {
        private readonly IRankingService _rankingService;

        public RankingController(IRankingService rankingService)
        {
            _rankingService = rankingService;
        }

        [HttpPost]
        public async Task<ActionResult<AddPlayerRankingResponse>> AddPlayerRanking([FromBody] AddPlayerRankingRequest? req)
        {
            if (req == null || string.IsNullOrEmpty(req.PlayerId))
                return BadRequest("PlayerId is required.");

            try
            {
                var data = new PlayerRankingData
                {
                    PlayerId = req.PlayerId,
                    PlayerScore = req.PlayerScore
                };

                await _rankingService.AddPlayerRankingAsync(data);

                var ranking = await _rankingService.GetRankingByPlayerIdAsync(req.PlayerId);
                
                if (ranking == null)
                    return NotFound("Player ranking is not found.");
                
                var response = new AddPlayerRankingResponse
                {
                    PlayerId = ranking.PlayerId,
                    PlayerScore = ranking.PlayerScore,
                    Ranking = ranking.Ranking
                };
                
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRankings()
        {
            try
            {
                var rankings = await _rankingService.GetAllRankingsAsync();
                var datas = rankings.ToList();
                var response = new
                {
                    Count = datas.Count(),
                    Rankings = datas
                };
                
                return Ok(response);
            }   
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetRankingByPlayerId(string playerId)
        {
            if (string.IsNullOrEmpty(playerId))
                return BadRequest("PlayerId is required.");

            try
            {
                var ranking = await _rankingService.GetRankingByPlayerIdAsync(playerId);
                if (ranking == null)
                    return NotFound("Player ranking is not found.");

                var response = new
                {
                    PlayerId = ranking.PlayerId,
                    PlayerScore = ranking.PlayerScore,
                    Ranking = ranking.Ranking
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }
    }
}
