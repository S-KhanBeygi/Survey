﻿namespace DaraSurvey.Models
{
    public class ChangePasswordReq
    {
        public string CurrentPassword { get; set; }

        public string NewPassword { get; set; }
    }
}
