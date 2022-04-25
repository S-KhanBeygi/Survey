using DaraSurvey.WidgetServices.Models;
using System;
using System.Collections.Generic;

namespace DaraSurvey.Widgets.Images
{
    public class ViewModel : ViewModelBase
    {
        public IEnumerable<Item> Items { get; set; }

        public override bool UserResponseIsValid(string userResponse)
        {
            throw new NotImplementedException();
        }
    }
}