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
                return StatusCode(500, "Server Error");
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
    }
}