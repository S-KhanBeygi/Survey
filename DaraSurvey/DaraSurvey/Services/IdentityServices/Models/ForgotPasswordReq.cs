using System.ComponentModel.DataAnnotations;

namespace DaraSurvey.Models
{
    public class ForgotPasswordReq
    {
        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string email { get; set; }
    }
}
