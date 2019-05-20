using Kira.LaconicInvoicing.Identity.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Controllers
{
    public class Test3Controller : BaseApiController
    {
        private readonly IRepository<LoginLog, Guid> _loginLogRepo;

        /// <summary>
        /// 初始化一个<see cref="Test3Controller"/>类型的新实例
        /// </summary>
        public Test3Controller(IRepository<LoginLog, Guid> loginLogRepository)
        {
            _loginLogRepo = loginLogRepository;
        }

        [HttpGet]
        [AllowAnonymous]
        [Description("性能测试获取数据")]
        public async Task<AjaxResult> Get()
        {
            try
            {
                return new AjaxResult().Success(await _loginLogRepo.Query().FirstOrDefaultAsync());
            }
            catch
            {
                return new AjaxResult().Error();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("性能测试-添加数据")]
        public async Task<AjaxResult> Add()
        {
            try
            {
                var log = new LoginLog()
                {
                    Id = Guid.NewGuid(),
                    UserId = 6,
                    CreatedTime = DateTime.Now
                };
                await _loginLogRepo.InsertAsync(log);
                return new AjaxResult().Success();
            }
            catch
            {
                return new AjaxResult().Error();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("性能测试-删除数据")]
        public async Task<AjaxResult> Delete()
        {
            try
            {
                var log = await _loginLogRepo.Query().FirstOrDefaultAsync();
                if (log != null)
                    await _loginLogRepo.DeleteAsync(log);
                return new AjaxResult().Success();
            }
            catch
            {
                return new AjaxResult().Error();
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("性能测试-更新数据")]
        public async Task<AjaxResult> Update()
        {
            try
            {
                var log = await _loginLogRepo.Query().FirstOrDefaultAsync();
                if (log != null)
                {
                    log.CreatedTime = DateTime.Now;
                    await _loginLogRepo.UpdateAsync(log);
                }
                return new AjaxResult().Success();
            }
            catch
            {
                return new AjaxResult().Error();
            }
        }
    }
}
