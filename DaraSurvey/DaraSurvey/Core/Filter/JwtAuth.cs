using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;

namespace DaraSurvey.Core.Filters
{
    public class JwtAuth : AuthorizeAttribute, IOrderedFilter
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

        public int Order => int.MaxValue;

        // --------------------

        private void Init()
        {
            this.AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme;
        }
    }
}
