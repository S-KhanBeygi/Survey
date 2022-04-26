using DaraSurvey.WidgetServices.Models;

namespace DaraSurvey.WidgetServices
{
    public interface IWidgetService
    {
        ViewModelBase GetWidget(string widgetData);
    }
}