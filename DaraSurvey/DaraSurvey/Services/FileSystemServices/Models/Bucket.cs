using System;

namespace DaraSurvey.Models
{
    public class Bucket
    {
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public long Size { get; set; }
    }
}
