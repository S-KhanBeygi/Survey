using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace DaraSurvey.Models
{
    public class JwtAuth : AuthorizeAttribute
    {
        public JwtAuth()
        {
            Init();
        }

        // --------------------

        public JwtAuth(string roles)
        {
            Init();
            this.Roles = roles;
        }

        // --------------------

        private void Init()
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
