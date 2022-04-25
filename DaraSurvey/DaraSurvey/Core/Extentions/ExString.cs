using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace DaraSurvey.Core
{
    public static class ExString
    {
        public static string FixString(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return str;

            var replacements = new Dictionary<string, string>()
            {
                {"ي","ی"},
                {"ك","ک"},
                {"‌", " "},
                {HttpUtility.UrlDecode("%C2%A0"), " "},
                {HttpUtility.UrlDecode("%A0"), " "}
            };

            var regex = new Regex(String.Join("|", replacements.Keys.Select(k => Regex.Escape(k))));
            str = regex.Replace(str, m => replacements[m.Value]);

            return str
                .Trim('\r')
                .Trim('\n')
                .Trim('\t')
                .Trim();
        }

        // ------------------------

        public static string GetString(this IEnumerable<string> query, string seperator = ", ")
        {
            if (query.Count() == 0)
                return "";

            return string.Join(seperator, query);
        }

        // --------------------

        public static string UppercaseFirst(this string str)
        {
            return (string.IsNullOrEmpty(str))
                ? string.Empty
                : char.ToUpper(str[0]) + str.Substring(1);
        }
    }
}
