using OSharp.AspNetCore.Mvc;
using System;

namespace Kira.LaconicInvoicing.Web.Controllers
{
    public class BaseApiController : ApiController
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
