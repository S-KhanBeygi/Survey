using DaraSurvey.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DaraSurvey.Extentions
{
    public static class ExRole
    {
        public static IEnumerable<Role> GetRoles()
        {
            return Enum.GetValues(typeof(Role)).Cast<Role>().ToList();
        }

        // ----------------------

        public static IEnumerable<string> GetStringRoles()
        {
            return Enum.GetValues(typeof(Role)).Cast<Role>().Select(o => o.ToString()).ToList();
        }
    }
}
