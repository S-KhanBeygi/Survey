using DaraSurvey.Core;
using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Widgets.Images
{
    public class Item
    {
        [Required]
        public string Id { get; set; }

        [Attachment("Widgets", "Image")]
        public string Image { get; set; }

        public string Title { get; set; }

        public string Url { get; set; }
    }
}