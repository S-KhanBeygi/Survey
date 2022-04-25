using DaraSurvey.Interfaces;
using DaraSurvey.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DaraSurvey.Controllers
{
    [ApiController]
    [Route("api/v1/roles")]
    [Produces("application/json")]
    [JwtAuth("root")]
    public class RolesController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RolesController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        // ------------------------

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_roleService.GetAll().Select(o => o.Name));
        }
    }
}
