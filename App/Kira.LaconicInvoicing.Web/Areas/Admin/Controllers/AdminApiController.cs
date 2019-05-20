using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc;
using OSharp.Core;


namespace Kira.LaconicInvoicing.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [RoleLimit]
    public abstract class AdminApiController : AreaApiController
    { }
}