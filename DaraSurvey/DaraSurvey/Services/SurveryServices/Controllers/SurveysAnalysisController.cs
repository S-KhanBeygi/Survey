using AutoMapper;
using DaraSurvey.Core.Filters;
using DaraSurvey.Services.SurveryServices;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/suerveys/{serveyId}/Analysis")]
    [JwtAuth(Roles = "root, surveys")]
    public class SurveysAnalysisController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public SurveysAnalysisController(IQuestionService questionService, IMapper mapper)
        {
            _questionService = questionService;
            _mapper = mapper;
        }

        // --------------------

        [HttpGet]
        public ActionResult<IEnumerable<QuestionRes>> GetAll([FromQuery] QuestionOrderedFilter model)
        {
            var entities = _questionService.GetAll(model, true);
            var result = _mapper.Map<IEnumerable<QuestionRes>>(entities);
            return Ok(result);
        }
    }
}