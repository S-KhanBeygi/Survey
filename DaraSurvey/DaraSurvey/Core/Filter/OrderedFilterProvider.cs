using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace DaraSurvey.Core.Filter
{
    public class OrderedFilterProvider : IFilterProvider
    {
        public int Order => throw new NotImplementedException();

        public void OnProvidersExecuted(FilterProviderContext context)
        {
            throw new NotImplementedException();
        }

        public void OnProvidersExecuting(FilterProviderContext context)
        {
            throw new NotImplementedException();
        }
    }
}
