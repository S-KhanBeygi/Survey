using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Linq;
using System.Security.Claims;

namespace DaraSurvey.Extentions
{
    public static class ExIdentity
    {
        public static string GetErrorMessage(this IdentityResult result)
        {
            if (result.Succeeded)
                return "Succeeded";
            else
                return String.Join(" | ", result.Errors.Select(e => e.Description));
        }

        // ----------------------

        public static string GetUserId(this HttpRequest request)
        {
            var claim = GetClaim(request, ClaimTypes.NameIdentifier);
            return claim?.Value;
        }

        // ----------------------

        public static Claim GetClaim(this HttpRequest request, string claimType)
        {
            var identityClaims = request.HttpContext.User.Identity as ClaimsIdentity;
            return identityClaims?.FindFirst(claimType);
        }
    }
}
