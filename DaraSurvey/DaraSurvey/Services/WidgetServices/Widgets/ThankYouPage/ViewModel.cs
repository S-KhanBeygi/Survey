using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Widgets.ThankYouPage
{
    public class ViewModel : ViewModelBase
    {
        public DaraSurvey.Widgets.Text.ViewModel Text { get; set; }

        public DaraSurvey.Widgets.Images.ViewModel Image { get; set; }

        public string ReturnUrl { get; set; }

        public override bool UserResponseIsValid(string userResponse)
        {
            throw new System.NotImplementedException();
        }
    }
}
