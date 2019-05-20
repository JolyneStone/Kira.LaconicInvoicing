using System;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Security;
using Kira.LaconicInvoicing.Service.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc;
using OSharp.Caching;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Entity;


namespace Kira.LaconicInvoicing.Web.Areas.Admin.Controllers
{
    [ModuleInfo(Order = 1)]
    [Description("管理-信息汇总")]
    public class DashboardController : AdminApiController
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly SecurityManager _securityManager;
        private readonly ICacheService _cacheService;

        /// <summary>
        /// 初始化一个<see cref="DashboardController"/>类型的新实例
        /// </summary>
        public DashboardController(UserManager<User> userManager,
            RoleManager<Role> roleManager,
            SecurityManager securityManager,
            ICacheService cacheService)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _securityManager = securityManager;
            _cacheService = cacheService;
        }

        /// <summary>
        /// 获取统计数据
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">结束时间</param>
        /// <returns>统计数据</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("统计数据")]
        public IActionResult SummaryData(DateTime start, DateTime end)
        {
            IFunction function = this.GetExecuteFunction();
            Expression<Func<User, bool>> userExp = GetExpression<User>(start, end);

            var users = _cacheService.ToCacheList(_userManager.Users.Where(userExp).GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(m => 1),
                ValidCount = g.Count(n => n.EmailConfirmed)
            }), function, "Dashboard_Summary_User", start, end).FirstOrDefault()
                ?? new { TotalCount = 0, ValidCount = 0 };

            var roles = _cacheService.ToCacheList(_roleManager.Roles.GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(m => 1),
                AdminCount = g.Count(m => m.IsAdmin)
            }), function, "Dashboard_Summary_Role", start, end).FirstOrDefault()
                ?? new { TotalCount = 0, AdminCount = 0 };
            var modules = _cacheService.ToCacheList(_securityManager.Modules.GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(m => 1),
                SiteCount = g.Count(m => m.TreePathString.Contains("$2$")),
                AdminCount = g.Count(m => m.TreePathString.Contains("$3$"))
            }), function, "Dashboard_Summary_Module").FirstOrDefault()
                ?? new { TotalCount = 0, SiteCount = 0, AdminCount = 0 };
            var functions = _cacheService.ToCacheList(_securityManager.Functions.GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(m => 1),
                ControllerCount = g.Count(m => m.IsController)
            }), function, "Dashboard_Summary_Function").FirstOrDefault()
                ?? new { TotalCount = 0, ControllerCount = 0 };
            var entityInfos = _cacheService.ToCacheList(_securityManager.EntityInfos.GroupBy(m => 1).Select(g => new
            {
                TotalCount = g.Sum(m => 1),
                AuditCount = g.Count(m => m.AuditEnabled)
            }), function, "Dashboard_Summary_EntityInfo").FirstOrDefault()
                ?? new { TotalCount = 0, AuditCount = 0 };

            var data = new { users, roles, modules, functions, entityInfos };
            return Json(data);
        }

        [HttpGet]
        [ModuleInfo]
        [Description("曲线数据")]
        public IActionResult LineData(DateTime start, DateTime end)
        {
            IFunction function = this.GetExecuteFunction();
            Expression<Func<User, bool>> userExp = GetExpression<User>(start, end);

            var userData = _cacheService.ToCacheList(_userManager.Users.Where(userExp).GroupBy(m => m.CreatedTime.Date).Select(g => new
            {
                Date = g.Key,
                DailyCount = g.Count()
            }), function, "Dashboard_Line_User", start, end);
            var users = userData.Select(m => new
            {
                Date = m.Date.ToString("d"),
                m.DailyCount,
                DailySum = userData.Where(n => n.Date <= m.Date).Sum(n => n.DailyCount)
            }).ToList();

            return Json(users);
        }

        private static Expression<Func<TEntity, bool>> GetExpression<TEntity>(DateTime start, DateTime end)
            where TEntity : class, ICreatedTime
        {
            if (start > end)
            {
                throw new ArgumentException($"结束时间{end}不能小于开始时间{start}");
            }
            return m => m.CreatedTime.Date >= start.Date && m.CreatedTime.Date <= end.Date;
        }
    }
}