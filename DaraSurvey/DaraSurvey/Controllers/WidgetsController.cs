using DaraSurvey.Core.Filter;
using DaraSurvey.Core.Request;
using DaraSurvey.Extentions;
using DaraSurvey.Models;
using DaraSurvey.Services.SurveryServices.Entities;
using DaraSurvey.Services.SurveryServices.Models;
using DaraSurvey.WidgetServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.WidgetServices.Controllers
{
    [Route("api/v1/widgets")]
    public class WidgetsController : ControllerBase
    {
        private IWidgetService _widgetService;
        public WidgetsController(IWidgetService widgetService)
        {
            _widgetService = widgetService;
        }

        // --------------------

        [HttpGet("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<ViewModelBase> Get([FromRoute] int id)
        {
            var result = _widgetService.GetViewModel(id);

            return Ok(result);
        }

        // --------------------

        [HttpPost]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<ViewModelBase> Create([FromBody, ModelBinder(typeof(EditModelBinder))] EditModelBase model)
        {
            var result = _widgetService.Create(model);

            return Ok(result);
        }

        // --------------------

        [HttpPut("{id}")]
        [MockUser(Role = Role.root)]
        [MockAuth(Roles = "root")]
        public ActionResult<ViewModelBase> Update([FromRoute] int id, [FromBody, ModelBinder(typeof(EditModelBinder))] EditModelBase model)
        {
            var result = _widgetService.Updata(id, model);

            return Ok(result);
        }

        // --------------------

        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _widgetService.Delete(id);
            return NoContent();
        }
    }
}