using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DaraSurvey.Core.Filters
{
    public class JwtAuth : AuthorizeAttribute
    {
        public JwtAuth()
        {
            Init();
        }

        // --------------------

        public JwtAuth(string policy)
        {
            Init();
            this.Policy = policy;
        }

        // --------------------

        private void Init()
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
