
namespace DaraSurvey.Models
{
    public class userProfileReq
    {
        public UserReq User { get; set; }
        public ProfileReq Profile { get; set; }
    }

    // --------------------

    public class userProfileOut
    {
        public UserRes User { get; set; }
        public ProfileReq Profile { get; set; }
    }
}
