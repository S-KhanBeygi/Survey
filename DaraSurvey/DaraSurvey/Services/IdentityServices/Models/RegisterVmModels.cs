using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Models
{
    public class RegisterReq
    {
        public UserReq User { get; set; }

        public ProfileReq Profile { get; set; }

        [StringLength(100, MinimumLength = 6)]
        [Required]
        public string Password { get; set; }
    }

    // --------------------

    public class RegisterRes
    {
        public string Id { get; set; }
        public string Token { get; set; }
    }
}
