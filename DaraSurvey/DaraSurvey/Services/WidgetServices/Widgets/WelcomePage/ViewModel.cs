using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.Widgets.WelcomePage
{
    public class ViewModel : ViewModelBase
    {
        public string Text { get; set; }

        public DaraSurvey.Widgets.Images.Item Image { get; set; }


        public override bool UserResponseIsValid(string userResponse)
        {
            throw new System.NotImplementedException();
        }
    }
}
