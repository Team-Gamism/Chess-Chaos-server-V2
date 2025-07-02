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
        public async Task<ActionResult<AddPlayerRankingResponse>> AddPlayerRanking(
            [FromBody] AddPlayerRankingRequest? req,
            [FromServices] ISessionService sessionService)
        {
            if (req == null || string.IsNullOrEmpty(req.PlayerId))
                return BadRequest("PlayerId is required.");
            
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                return BadRequest("No authorization header");

            const string prefix = "Session ";
            var authHeaderValue = authHeader.ToString();
            if (!authHeaderValue.StartsWith(prefix))
                return BadRequest("Invalid session format.");

            var sessionId = authHeaderValue[prefix.Length..];
            
            if (!await sessionService.ValidateSessionAsync(sessionId))
                return Unauthorized("Session is not valid.");

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
                var data = rankings.ToList();
                var response = new AllPlayerRankingResponse
                {
                    Count = data.Count(),
                    Rankings = data
                };
                
                return Ok(response);
            }   
            catch (Exception ex)
            {
                return StatusCode(500, "server error: " + ex.Message);
            }
        }

        [HttpGet("{playerId}")]
        public async Task<IActionResult> GetRankingByPlayerId(
            string playerId,
            [FromServices] ISessionService sessionService)
        {
            if (string.IsNullOrEmpty(playerId))
                return BadRequest("PlayerId is required.");

            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                return BadRequest("No authorization header");

            const string prefix = "Session ";
            var authHeaderValue = authHeader.ToString();
            if (!authHeaderValue.StartsWith(prefix))
                return BadRequest("Invalid session format.");

            var sessionId = authHeaderValue[prefix.Length..];
            
            if (!await sessionService.ValidateSessionAsync(sessionId))
                return Unauthorized("Session is not valid.");
            
            try
            {
                var ranking = await _rankingService.GetRankingByPlayerIdAsync(playerId);
                if (ranking == null)
                    return NotFound("Player ranking is not found.");

                var response = new PlayerRankingResponse
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
