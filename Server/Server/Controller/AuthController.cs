using Microsoft.AspNetCore.Mvc;
using Server.Model.Account.Dto.Request;
using Server.Model.Account.Dto.Response;
using Server.Service.Interface;

namespace Server.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<ActionResult<PlayerRegisterResponse>> Register([FromBody] PlayerRegisterRequest request)
        {
            try
            {
                var result = await _authService.RegisterAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error" + ex.Message);
            }
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<PlayerLoginResponse>> Login([FromBody] PlayerLoginRequest req)
        {
            try
            {
                var result = await _authService.LoginAsync(req);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return Unauthorized(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error" + ex.Message);
            }
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authHeader))
                return BadRequest("No authorization header");
                
            const string prefix = "Session ";
            if (!authHeader.ToString().StartsWith(prefix))
                return BadRequest("Invalid session format.");
            
            var sessionId = authHeader.ToString()[prefix.Length..];
            
            try
            {
                await _authService.LogoutAsync(sessionId);
                return Ok("Logout");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Server Error" + ex.Message);
            }
        }
    }
}