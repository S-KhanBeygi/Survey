namespace DaraSurvey.Core.Helpers
{
    public static class PhoneNumberHelper
    {
        public static string GetFullPhoneNumber(int phoneCode, long phoneNumber)
        {
            return $"{phoneCode}{phoneNumber}";
        }
    }
}
