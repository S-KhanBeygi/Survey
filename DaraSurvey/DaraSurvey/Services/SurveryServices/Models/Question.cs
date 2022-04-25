using DaraSurvey.Core.BaseClasses;
using DaraSurvey.WidgetServices.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace DaraSurvey.Services.SurveryServices.Models
{
    public abstract class QuestionDtoBase
    {
        public string Text { get; set; }

        [BindProperty(BinderType = typeof(EditModelBinder))]
        public EditModelBase Widget { get; set; }

        public int SurveyId { get; set; }

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
