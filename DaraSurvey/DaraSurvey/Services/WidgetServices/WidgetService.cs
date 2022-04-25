using DaraSurvey.Core;
using DaraSurvey.Core.BaseClasses;
using DaraSurvey.Core.Helpers;
using DaraSurvey.WidgetServices.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DaraSurvey.WidgetServices
{
    public class WidgetService : IWidgetService
    {
        public WidgetService() { }

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