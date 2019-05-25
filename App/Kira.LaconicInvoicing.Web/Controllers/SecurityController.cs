using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Kira.LaconicInvoicing.Security;
using Kira.LaconicInvoicing.Security.Entities;
using Kira.LaconicInvoicing.Service.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Caching;
using OSharp.AspNetCore.Mvc;
using OSharp.Collections;
using OSharp.Core;
using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Secutiry;
using Microsoft.EntityFrameworkCore;

namespace Kira.LaconicInvoicing.Web.Controllers
{
    [Description("网站-授权")]
    [ModuleInfo(Order = 2)]
    [RoleLimit]
    public class SecurityController : BaseApiController
    {
        private readonly SecurityManager _securityManager;
        private readonly ILogger<SecurityController> _logger;

        public SecurityController(
            SecurityManager securityManager,
            ILoggerFactory loggerFactory)
        {
            _securityManager = securityManager;
            _logger = loggerFactory.CreateLogger<SecurityController>();
        }

        /// <summary>
        /// 检查URL授权
        /// </summary>
        /// <param name="url">要检查的URL</param>
        /// <returns>是否有权</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("检查URL授权")]
        public bool CheckUrlAuth(string url)
        {
            bool ok = this.CheckFunctionAuth(url);
            return ok;
        }

        /// <summary>
        /// 获取授权信息
        /// </summary>
        /// <returns>权限节点</returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取授权信息")]
        //public List<string> GetAuthInfo()
        public async Task<IActionResult> GetAuthInfo()
        {
            try
            {
                // 出于性能考虑，将EF导航查询更改为关联查询
                var cache = ServiceProvider.GetService<IDistributedCache>();
                var key = $"{User.Identity.Name}.AuthInfo";
                DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
                options.SetSlidingExpiration(TimeSpan.FromMinutes(120));
                var authInfo = await cache.GetAsync<List<string>>(key,
                            async () =>
                            {
                                IFunctionAuthorization authorization = ServiceProvider.GetService<IFunctionAuthorization>();
                                List<AuthItem> list = new List<AuthItem>();
                                Module[] modules = await _securityManager.Modules.ToArrayAsync();
                                foreach (Module module in modules)
                                {
                                    if (CheckFuncAuth(module, authorization, out bool empty))
                                    {
                                        list.Add(new AuthItem { Code = GetModuleTreeCode(module, modules), HasFunc = !empty });
                                    }
                                }

                                //var moduleFunctions = await (from module in _securityManager.Modules
                                //                       join moduleFunction in _securityManager.ModuleFunctions
                                //                       on module.Id equals moduleFunction.ModuleId
                                //                       join function in _securityManager.Functions
                                //                       on moduleFunction.FunctionId equals function.Id into f
                                //                       select new { Module = module, Functions = f }).ToListAsync();

                                //var modules = moduleFunctions.Select(m => m.Module).ToArray();

                                foreach (var item in modules)
                                {
                                    if (CheckFuncAuth(item, authorization, out bool empty))
                                    {
                                        list.Add(new AuthItem { Code = GetModuleTreeCode(item, modules), HasFunc = !empty });
                                    }
                                }

                                List<string> codes = new List<string>();
                                foreach (AuthItem item in list)
                                {
                                    if (item.HasFunc)
                                    {
                                        codes.Add(item.Code);
                                    }
                                    else if (list.Any(m => m.Code.Length > item.Code.Length && m.Code.Contains(item.Code) && m.HasFunc))
                                    {
                                        codes.Add(item.Code);
                                    }
                                }
                                return codes;
                            },
                            options);

                return Json(authInfo);
            }
            catch (Exception ex)
            {
                return Json(ex.GetBaseException().Message);
            }
        }

        /// <summary>
        /// 验证是否拥有指定模块的权限
        /// </summary>
        /// <param name="functions">要验证的功能集合</param>
        /// <param name="authorization">功能权限验证接口</param>
        /// <param name="empty">返回模块是否为空模块，即是否分配有功能</param>
        /// <returns></returns>
        private bool CheckFuncAuth(IEnumerable<Function> functions, IFunctionAuthorization authorization, out bool empty)
        {
            empty = functions.Count() == 0;
            if (empty)
            {
                return true;
            }

            foreach (Function function in functions)
            {
                if (!authorization.Authorize(function, User).IsOk)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 验证是否拥有指定模块的权限
        /// </summary>
        /// <param name="module">要验证的模块</param>
        /// <param name="authorization">功能权限验证接口</param>
        /// <param name="empty">返回模块是否为空模块，即是否分配有功能</param>
        /// <returns></returns>
        private bool CheckFuncAuth(Module module, IFunctionAuthorization authorization, out bool empty)
        {
            Function[] functions = _securityManager.ModuleFunctions.Where(m => m.ModuleId == module.Id).Select(m => m.Function).ToArray();
            empty = functions.Length == 0;
            if (empty)
            {
                return true;
            }

            foreach (Function function in functions)
            {
                if (!authorization.Authorize(function, User).IsOk)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 获取模块的树形路径代码串
        /// </summary>
        private static string GetModuleTreeCode(Module module, Module[] source)
        {
            var pathIds = module.TreePathIds;
            //string[] names = pathIds.Select(m => source.First(n => n.Id == m)).Select(m => m.Code).ToArray();
            var names = from p in pathIds
                        join m in source
                        on p equals m.Id
                        select m.Code;
            return names.ExpandAndToString(".");
        }


        private class AuthItem
        {
            public string Code { get; set; }

            public bool HasFunc { get; set; }
        }
    }
}