using AutoMapper;
using DaraSurvey.Core.Request;
using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace DaraSurvey.Controllers
{

    [ApiController]
    [Route("api/v1/account")]
    [Produces("application/json")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        private readonly IMapper _mapper;
        public AccountController(IAccountService accountService, IMapper mapper)
        {
            _accountService = accountService;
            _mapper = mapper;
        }

        // ------------------------

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<LoginRes>> Login([FromBody] LoginReq model)
        {
            var result = await _accountService.LoginAsync(model);
            return Ok(result);
        }

        // ------------------------

        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<RegisterRes>> RegisterAsync([FromBody] RegisterReq model)
        {
            var result = await _accountService.RegisterAsync(model);
            return Ok(result);
        }

        // ------------------------

        [HttpPost("logout")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        [JwtAuth]
        public async Task<ActionResult> Logout()
        {
            await _accountService.LogoutAsync();
            return NoContent();
        }

        // ------------------------

        [HttpPost("forgot-password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordReq model)
        {
            await _accountService.ForgotPasswordAsync(model, Request);
            return NoContent();
        }

        // ------------------------

        [HttpPost("reset-password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordVmIn model)
        {
            await _accountService.ResetPasswordAsync(model);
            return NoContent();
        }

        // ------------------------

        [JwtAuth]
        [HttpPut("change-password")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordReq model)
        {
            var userId = Request.GetUsrId();
            await _accountService.ChangePasswordAsync(userId, model);
            return NoContent();
        }

        // ------------------------

        [HttpGet("profile")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [JwtAuth]
        public ActionResult<UserRes> GetProfile()
        {
            var result = _accountService.GetProfile(Request.GetUsrId());
            var outgoing = _mapper.Map<UserRes>(result);
            return Ok(outgoing);
        }
    }
}
