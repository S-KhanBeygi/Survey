using System;
using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Models
{
    public class UserReq
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        public int? CountryCode { get; set; }

        public string PhoneNumber { get; set; }
    }

    // --------------------

    public class UserRes : UserReq
    {
        public string Id { get; set; }

        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }

        public DateTime Created { get; set; }

        public ProfileRes Profile { get; set; }
    }
}

