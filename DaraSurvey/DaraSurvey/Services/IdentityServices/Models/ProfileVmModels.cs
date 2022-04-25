using DaraSurvey.Entities;
using System;

namespace DaraSurvey.Models
{
    public class ProfileReq
    {
        public Gender? Gender { get; set; }

        public DateTime? BirthDate { get; set; }

        public string Image { get; set; }

        public string NationalCode { get; set; }
    }

    // --------------------

    public class ProfileRes : ProfileReq
    {
        public string Id { get; set; }
    }
}
