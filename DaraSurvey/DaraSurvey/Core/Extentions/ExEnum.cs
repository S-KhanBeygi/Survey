using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace DaraSurvey.Core
{
    public static class ExEnum
    {
        public static string GetDisplayName(this Enum value)
        {
            if (value.ToString() == "0")
                return null;

            var enumType = value.GetType();
            var enumValue = Enum.GetName(enumType, value);
            var member = enumType.GetMember(enumValue)[0];

            var attrs = member.GetCustomAttributes(typeof(DisplayAttribute), false);
            if (attrs.Any())
            {
                var outString = ((DisplayAttribute)attrs[0]).Name;

                if (((DisplayAttribute)attrs[0]).ResourceType != null)
                    outString = ((DisplayAttribute)attrs[0]).GetName();

                return outString;
            }

            return null;
        }
    }
}
