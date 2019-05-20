using Kira.LaconicInvoicing.Notification.Dtos;
using Kira.LaconicInvoicing.Service.Notification;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Notification.Controllers
{
    [ModuleInfo(Order = 1, Position = "Notification", PositionName = "通知模块")]
    [Description("管理-通知")]
    public class NotificationController : NotificationApiController
    {
        private readonly INoticeClientContract _noticeClientContract;


        /// <summary>
        /// 初始化一个<see cref="NotificationController"/>类型的新实例
        /// </summary>
        public NotificationController(INoticeClientContract noticeClientContract)
        {
            _noticeClientContract = noticeClientContract;
        }

        /// <summary>
        /// 添加通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("添加通知")]
        public async Task<AjaxResult> Add(NoticeInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                dto.Id = Guid.NewGuid();
                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }

                dto.DateTime = DateTime.Now;
                await _noticeClientContract.AddNoticeAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 删除用户通知
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除用户通知")]
        public async Task<AjaxResult> DeleteByUser(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                await _noticeClientContract.DeleteNoticeByUserAsync(id, User.Identity.Name);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 删除用户所有通知
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除用户所有通知")]
        public async Task<AjaxResult> ClearAllByUser()
        {
            return await AjaxResult.Business(async result =>
            {
                await _noticeClientContract.DeleteAllNoticeByUserAsync(User.Identity.Name);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 删除通知
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除通知")]
        public async Task<AjaxResult> Delete(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                await _noticeClientContract.DeleteNoticeAync(id);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 删除所有通知
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除所有通知")]
        public async Task<AjaxResult> ClearAll()
        {
            return await AjaxResult.Business(async result =>
            {
                await _noticeClientContract.DeleteAllNoticeAync();
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 获取所有通知
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取所有通知")]
        public async Task<AjaxResult> GetAll(int count = 0, int size = 5)
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _noticeClientContract.GetNoticeAllAsync(count, size));
            });
        }

        /// <summary>
        /// 获取用户所有通知
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取用户所有通知")]
        public async Task<AjaxResult> GetAllByUser(int count = 0, int size = 5)
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _noticeClientContract.GetNoticeAllByUserAsync(User.Identity.Name, count, size));
            });
        }


        /// <summary>
        /// 获取用户所有未读通知
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取用户所有未读通知")]
        public async Task<AjaxResult> GetAllUnRead()
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _noticeClientContract.GetAllUnReadAsync(User.Identity.Name));
            });
        }

        /// <summary>
        /// 获取用户所有未读通知数
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取用户所有未读通知数")]
        public async Task<AjaxResult> GetAllUnReadCount()
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _noticeClientContract.GetAllUnReadCountAsync(User.Identity.Name));
            });
        }

        /// <summary>
        /// 阅读通知
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("阅读通知")]
        public async Task<AjaxResult> ReadNotice(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                await _noticeClientContract.ReadNoticeAsync(User.Identity.Name, id);
                result.Success();
            });
        }
    }
}
