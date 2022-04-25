using DaraSurvey.Core.Filters;
using DaraSurvey.Core.Request;
using DaraSurvey.Extentions;
using DaraSurvey.Services.SurveryServices;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/user-surveys")]
    public class UserSurveysController : ControllerBase
    {
        private IUserSurveyService _userSurveyService;
        public UserSurveysController(IUserSurveyService userSurveyService)
        {
            _userSurveyService = userSurveyService;
        }

        // --------------------

        [HttpGet]
        [JwtAuth]
        public ActionResult<IEnumerable<UsersSurvey>> GetOverview([FromQuery] UserSurveyOrderedFilter model)
        {
            if (!Request.GetUserRoles().Any(o => o == "root"))
                model.UserId = Request.GetUserId();

            var result = _userSurveyService.GetAll(model);

            return Ok(result);
        }

        // --------------------

        [HttpGet("count")]
        [JwtAuth]
        public ActionResult<int> GetOverviewCount([FromQuery] UserSurveyFilter model)
        {
            if (!Request.GetUserRoles().Any(o => o == "root"))
                model.UserId = Request.GetUserId();

            var result = _userSurveyService.Count(model);

            return Ok(result);
        }

        // --------------------

        [HttpPatch("{id}/approved")]
        [JwtAuth(Roles = "root, surveys")]
        public ActionResult<Survey> Count([FromRoute] int id, [FromQuery] bool approved)
        {
            _userSurveyService.Approved(id, approved);

            return NoContent();
        }
    }
}