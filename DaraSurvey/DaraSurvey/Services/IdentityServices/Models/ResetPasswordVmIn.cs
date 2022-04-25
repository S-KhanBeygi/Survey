namespace DaraSurvey.Models
{
    public class ResetPasswordVmIn
    {
        public string userId { get; set; }
        public string token { get; set; }
        public string newPassword { get; set; }
    }
}
