using Microsoft.AspNetCore.Identity;
using System;

namespace DaraSurvey.Entities
{
    public class User : IdentityUser
    {
        public short CountryCode { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime Created { get; set; }

        public DateTime? Updated { get; set; }

        public DateTime? Deleted { get; set; }

        public Profile Profile { get; set; }
    }
}
