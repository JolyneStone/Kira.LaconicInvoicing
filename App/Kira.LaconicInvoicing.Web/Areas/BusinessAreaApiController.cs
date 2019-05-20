using OSharp.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Mvc;
using OSharp.Core;

namespace Kira.LaconicInvoicing.Web.Areas
{
    [RoleLimit]
    public abstract class BusinessAreaApiController : AreaApiController
    {
        private IServiceProvider _serviceProvider;

        protected IServiceProvider ServiceProvider
        {
            get
            {
                if (_serviceProvider == null)
                {
                    _serviceProvider = HttpContext.RequestServices;
                }
                return _serviceProvider;
            }
        }
    }
}
