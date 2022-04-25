using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Models
{
    public class LoginReq
    {
        [Required]
        public string username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string password { get; set; }
    }

    // --------------------

    public class LoginRes
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? CountryCode { get; set; }
        public string PhoneNumber { get; set; }
        public string Token { get; set; }
        public long TokenTtl { get; set; }

        public int? CompanyId { get; set; }
        public IEnumerable<string> Roles { get; set; }
    }
}
