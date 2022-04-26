using DaraSurvey.Core.BaseClasses;
using DaraSurvey.WidgetServices.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public class QuestionDtoBase
    {
        private EditModelBase _widget;
        public string Text { get; set; }

        public EditModelBase Widget 
        {
            get { return _widget; }
            set { _widget = WidgetsDataDeserializer.Deserialize(JsonConvert.SerializeObject(value, JsonSeralizerSetting.SerializationSettings)); } 
        }

        public int SurveyId { get; set; }

        public bool IsRequired { get; set; }

        public bool IsCountable { get; set; }

        public string WidgetData
        {
            get
            {
                return JsonConvert.SerializeObject(Widget, JsonSeralizerSetting.SerializationSettings);
            }
        }
    }

    // --------------------

    public class QuestionCreation : QuestionDtoBase { }

    // --------------------

    public class QuestionUpdation : QuestionDtoBase { }


    // --------------------

    public class QuestionRes
    {
        public int Id { get; set; }

        public ViewModelBase Widget { get; set; }

        public bool IsRequired { get; set; }

        public string SurveyTitle { get; set; }

        public bool IsContable { get; set; }
    }
}
