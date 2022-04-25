using AutoMapper;
using DaraSurvey.Services.SurveryServices;
using DaraSurvey.Services.SurveryServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/questions")]
    //[JwtAuth(Roles = "root, surveys")]
    public class QuestionsController : ControllerBase
    {
        private readonly IQuestionService _questionService;
        private readonly IMapper _mapper;

        public QuestionsController(IQuestionService questionService, IMapper mapper)
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

        // --------------------

        [HttpGet("count")]
        public ActionResult<int> Count([FromQuery] QuestionFilter model)
        {
            var result = _questionService.Count(model);
            return Ok(result);
        }

        // --------------------

        [HttpGet("{id}")]
        public ActionResult<QuestionRes> Get([FromRoute] int id)
        {
            var entity = _questionService.Get(id, true);
            var result = _mapper.Map<QuestionRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPost]
        public ActionResult<QuestionRes> Create([FromBody] QuestionCreation model)
        {
            var entity = _questionService.Create(model);
            var result = _mapper.Map<QuestionRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpPut("{id}")]
        public ActionResult<QuestionRes> Update([FromRoute] int id, [FromBody] QuestionUpdation model)
        {
            var entity = _questionService.Update(id, model);
            var result = _mapper.Map<QuestionRes>(entity);
            return Ok(result);
        }

        // --------------------

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _questionService.Delete(id);
            return NoContent();
        }
    }
}