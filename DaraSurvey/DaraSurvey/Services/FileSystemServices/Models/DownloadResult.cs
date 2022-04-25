using System;

namespace DaraSurvey.Models
{
    public class DownloadResult
    {
        public Byte[] File { get; set; }
        public string MimeType { get; set; }
    }
}
