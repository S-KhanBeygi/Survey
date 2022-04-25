namespace DaraSurvey.Models
{
    public class UserUpdateModel : RegisterReq
    {
        public bool EmailConfirmed { get; set; }

        public bool PhoneNumberConfirmed { get; set; }
    }
}
