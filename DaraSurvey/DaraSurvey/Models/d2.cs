using DaraSurvey.WidgetServices.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DaraSurvey.Models
{
    public abstract class WidgetsDataDeserializer2
    {
        public static IEnumerable<EditModelBase> Deserialize(string serializedWidgets)
        {
            if (string.IsNullOrEmpty(serializedWidgets))
                return null;

            JArray jArray;

            try
            {
                jArray = (JArray)JsonConvert.DeserializeObject(serializedWidgets);
            }
            catch
            {
                throw new Exception("Deserializing failed");
            }

            if (jArray.Any(i => i["type"] == null))
            {

                throw new Exception("type field is required for all items");
            }

            var typeFormat = "Persina.Api.Widgets.{0}.EditModel, Persina";
            var binder = new TypeNameSerializationBinder(typeFormat);

            return jArray.Select(i => {
                var typeName = i["type"]?.ToString();

                if (typeName == null) throw new Exception("\"type\" field is required");

                var type = binder.BindToType(null, typeName);

                if (type == null) return null;

                return (EditModelBase)i.ToObject(type);
            })
            .Where(w => w != null);
        }
    }
}
