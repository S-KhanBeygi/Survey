using AutoMapper;
using DaraSurvey.Core;
using DaraSurvey.Core.BaseClasses;
using DaraSurvey.Core.Helpers;
using DaraSurvey.Entities;
using DaraSurvey.WidgetServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Net;

namespace DaraSurvey.WidgetServices
{
    public class WidgetService : IWidgetService
    {
        private readonly DB _db;
        private readonly IMapper _mapper;

        public WidgetService(DB db, IMapper mapper)
        {
            _db = db;
            _mapper = mapper;
        }

        // ------------------------

        public ViewModelBase GetViewModel(int id)
        {
            var widget = GetEntity(id);

            return GetWidget(widget.Data);
        }

        // ------------------------

        public Widget GetEntity(int id)
        {
            var widget =_db.Set<Widget>().SingleOrDefault(o => o.Id == id);
            if (widget == null)
                throw new ServiceException(HttpStatusCode.NotFound, ServiceExceptionCode.WidgetNotFound);

            return widget;
        }

        // ------------------------

        public ViewModelBase Create(EditModelBase model)
        {
            var widget = _mapper.Map<Widget>(model);

            _db.Set<Widget>().Add(widget);

            return GetWidget(widget.Data);
        }

        // ------------------------

        public ViewModelBase Updata(int id, EditModelBase model)
        {
            // todo:check some condition
            var widget = GetEntity(id);
            widget.Data = JsonConvert.SerializeObject(model, JsonSeralizerSetting.SerializationSettings);
            _db.Set<Widget>().Update(widget);
            _db.SaveChanges();

            return GetWidget(widget.Data);
        }

        // ------------------------

        public void Delete(int id)
        {
            // todo:check some condition
            var widget = GetEntity(id);
            _db.Set<Widget>().Remove(widget);
            _db.SaveChanges();
        }

        // ------------------------

        public ViewModelBase GetWidget(string widgetData)
        {
            var jToken = ((JToken)JsonConvert.DeserializeObject(widgetData, JsonSeralizerSetting.SerializationSettings));

            var typeFormat = "DaraSurvey.Widgets.{0}.ViewModel";
            var binder = new TypeNameSerializationBinder(typeFormat);

            var typeName = jToken["type"].ToString().UppercaseFirst();

            var type = binder.BindToType(null, typeName);

            if (type == null) return null;

            return (ViewModelBase)jToken.ToObject(type);
        }
    }
}