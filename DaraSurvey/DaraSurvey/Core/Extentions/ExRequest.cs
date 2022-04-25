using DaraSurvey.BaseClasses;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace DaraSurvey.Core.Request
{
    public static class ExRequest
    {
        public static string GetBaseUrl(this HttpRequest request)
        {
            return $"{request.Scheme}://{request.Host.Value}";
        }

        // --------------------

        public static string GetUsrId(this HttpRequest request)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            var calim = identityClaims?.FindFirst(IdentityClaimTypes.NameIdentifier);
            return calim?.Value;
        }

        // --------------------

        public static string GetUserName(this HttpRequest request)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            var calim = identityClaims?.FindFirst(IdentityClaimTypes.Name);
            return calim?.Value;
        }

        // --------------------

        public static int GetCompanyId(this HttpRequest request)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            var calim = identityClaims?.FindFirst(IdentityClaimTypes.CompanyId);
            return int.Parse(calim?.Value);
        }

        // --------------------

        public static IEnumerable<string> GetUserRoles(this HttpRequest request)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            var roles = identityClaims?.FindAll(IdentityClaimTypes.Role).Select(o => o.Value);
            return roles;
        }
    }
}
