using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DaraSurvey.Core
{
    public static class ExString
    {
        public static string UppercaseFirst(this string str)
        {
            return (string.IsNullOrEmpty(str))
                ? string.Empty
                : char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
