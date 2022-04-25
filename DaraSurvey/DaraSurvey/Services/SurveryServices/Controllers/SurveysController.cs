using AutoMapper;
using DaraSurvey.Core.Filters;
using DaraSurvey.Extentions;
using DaraSurvey.Services.SurveryServices;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/surveys")]
    public class SurveysController : ControllerBase
    {
        private readonly ISurveyService _surveyService;
        private readonly IUserSurveyService _userSurveyService;
        private readonly IMapper _mapper;

        public SurveysController(ISurveyService surveyService, IUserSurveyService userSurveyService, IMapper mapper)
        {
            _surveyService = surveyService;
            _userSurveyService = userSurveyService;
            _mapper = mapper;
        }

        // --------------------

        [HttpGet("overview")]
        //[JwtAuth]
        public ActionResult<IEnumerable<SurveyOverviewRes>> GetOverview([FromQuery] SurveyOverviewOrderedFilter model)
        {
            model.UserId = Request.GetUserId();
            var result = _surveyService.GetOverview(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("overview/count")]
        [JwtAuth]
        public ActionResult<int> GetOverviewCount([FromQuery] SurveyOverviewFilter model)
        {
            model.UserId = Request.GetUserId();
            var result = _surveyService.OverviewCount(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet]
        [JwtAuth(Roles = "root, surveys")]
        public ActionResult<Survey> GetAll([FromQuery] SurveyOrderedFilter model)
        {
            var result = _surveyService.GetAll(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("count")]
        [JwtAuth(Roles = "root, surveys")]
        public ActionResult<Survey> Count([FromQuery] SurveyFilter model)
        {
            var result = _surveyService.Count(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("{id}")]
        //[JwtAuth(Roles = "root, surveys")]
        public ActionResult<SurveyRes> Get([FromRoute] int id)
        {
            var entity = _surveyService.Get(id);
            var result = _mapper.Map<SurveyRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPost("{surveyId}/register")]
        [JwtAuth]
        public ActionResult Register([FromRoute] int surveyId)
        {
            _userSurveyService.Register(surveyId, Request.GetUserId());
            return NoContent();
        }

        // --------------------

        [HttpPost]
        //[JwtAuth(Roles = "root, surveys")]
        public ActionResult<SurveyRes> Create([FromBody] SurveyCreation model)
        {
            var entity = _surveyService.Create(model);
            var result = _mapper.Map<SurveyRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPut("{id}")]
        [JwtAuth(Roles = "root, surveys")]
        public ActionResult<SurveyRes> Update([FromRoute] int id, [FromBody] SurveyUpdation model)
        {
            var entity = _surveyService.Update(id, model);
            var result = _mapper.Map<SurveyRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpDelete("{id}")]
        [JwtAuth(Roles = "root, surveys")]
        public ActionResult Delete([FromRoute] int id)
        {
            _surveyService.Delete(id);
            return NoContent();
        }
    }
}