using System;
using System.Collections.Generic;

namespace DaraSurvey.Models
{
    public class Container
    {
        public string Name { get; set; }
        public DateTime? Created { get; set; }
        public IEnumerable<Bucket> Buckets { get; set; }
        public long Size { get; set; }
    }
}
