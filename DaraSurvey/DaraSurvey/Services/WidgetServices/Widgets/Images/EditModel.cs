using DaraSurvey.WidgetServices.Models;
using System.Collections.Generic;

namespace DaraSurvey.Widgets.Images
{
    public class EditModel : EditModelBase
    {
        public IEnumerable<Item> Items { get; set; }
    }
}