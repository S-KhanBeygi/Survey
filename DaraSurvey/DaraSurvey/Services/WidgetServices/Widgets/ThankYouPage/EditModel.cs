using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Widgets.ThankYouPage
{
    public class EditModel : EditModelBase
    {
        public DaraSurvey.Widgets.Text.EditModel Text { get; set; }

        public DaraSurvey.Widgets.Images.EditModel Image { get; set; }

        public string ReturnUrl { get; set; }
    }
}
