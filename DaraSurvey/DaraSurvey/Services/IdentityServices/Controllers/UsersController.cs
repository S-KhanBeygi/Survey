using AutoMapper;
using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace DaraSurvey.Controllers
{
    [ApiController]
    [Route("api/v1/users")]
    [Produces("application/json")]
    [JwtAuth("root, users")]
    public class UsersController : ControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IUserService _userService;

        public UsersController(IMapper mapper, IUserService userService)
        {
            _mapper = mapper;
            _userService = userService;
        }

        // ------------------------

        [HttpGet]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<IEnumerable<UserRes>> GetAll([FromQuery] UserOrderedFilter model)
        {
            var result = _userService.GetAll(model).ToList();

            var outgoing = _mapper.Map<IEnumerable<UserRes>>(result);

            return Ok(outgoing);
        }

        // ------------------------

        [HttpGet("count")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<int> Count([FromQuery] UserFilter model)
        {
            var result = _userService.Count(model);

            return Ok(result);
        }

        // ------------------------

        [HttpGet("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public ActionResult<UserRes> GetById([FromRoute] string id)
        {
            var result = _userService.GetById(id);

            var outgoing = _mapper.Map<UserRes>(result);

            return Ok(outgoing);
        }

        // ------------------------

        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserRes>> Create([FromBody] UserUpdateModel model)
        {
            var user = await _userService.CreateAsync(model);

            var outgoing = _mapper.Map<UserRes>(user);

            return Ok(outgoing);
        }

        // ------------------------

        [HttpDelete("{userId}")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> Delete([FromRoute] string userId)
        {
            await _userService.DeleteAsync(userId);

            return NoContent();
        }

        // ------------------------

        [HttpPut("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public async Task<ActionResult<UserRes>> UpdateAsync([FromRoute] string id, [FromBody] UserUpdateModel model)
        {
            var user = await _userService.UpdateAsync(id, model);

            var outgoing = _mapper.Map<UserRes>(user);

            return Ok(outgoing);
        }

        // --------------------

        [HttpGet("{id}/roles")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult<IEnumerable<string>>> GetRolesAsync([FromRoute] string id)
        {
            var result = await _userService.GetRolesAsync(id);

            return Ok(result);
        }

        // --------------------

        [HttpPut("{id}/roles")]
        [ProducesResponseType((int)HttpStatusCode.NoContent)]
        public async Task<ActionResult> AddToRolesAsync([FromRoute] string id, [FromBody] IEnumerable<string> roles)
        {
            await _userService.AddRolesToUserAsync(id, roles);

            return NoContent();
        }
    }
}
