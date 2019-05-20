using Kira.LaconicInvoicing.Identity.Dtos;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Infrastructure.Options;
using Kira.LaconicInvoicing.Security;
using Kira.LaconicInvoicing.Service.Identity;
using Kira.LaconicInvoicing.Service.UserCenter;
using Kira.LaconicInvoicing.UserCenter.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSharp.AspNetCore.UI;
using OSharp.Caching;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Identity;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.UserCenter.Controllers
{
    [ModuleInfo(Order = 1, Position = "UserCenter", PositionName = "用户中心模块")]
    [Description("管理-个人信息")]
    public class ProfileController : UserCenterApiController
    {
        private readonly IUserCenterContract _userCenterContract;

        /// <summary>
        /// 初始化一个<see cref="ProfileController"/>类型的新实例
        /// </summary>
        public ProfileController(IUserCenterContract userCenterContract)
        {
            _userCenterContract = userCenterContract;
        }

        /// <summary>
        /// 获取用户详细信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("用户详细信息")]
        public async Task<UserDetailOutputDto> Info()
        {
            UserDetailOutputDto userDetailDto = await _userCenterContract.GetUserDetailAsync(User.Identity.Name, ServiceProvider);
            
            return userDetailDto;
        }

        /// <summary>
        /// 更新用户详细信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("更新用户详细信息")]
        public async Task<AjaxResult> UpdateInfo(UserDetailInputDto userDetailDto)
        {
            Check.NotNull(userDetailDto, nameof(userDetailDto));
            var (result, obj) = await _userCenterContract.UpdateUserDetailAsync(userDetailDto);
            if (result)
            {
                ServiceProvider.GetService<IOnlineUserCache>()?.Remove(User.Identity.Name);
                return AjaxResult.CreateSuccess(obj);
            }
            else
                return AjaxResult.CreateError(obj);
        }


        /// <summary>
        /// 更新用户头像
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("更新用户头像")]
        public async Task<AjaxResult> UpdateAvatar(IFormFile avatarFile)
        {
            Check.NotNull(avatarFile, nameof(avatarFile));
            var options = ServiceProvider.GetRequiredService<IOptions<ApplicationOptions>>()?.Value;
            //if (string.IsNullOrWhiteSpace(options.FrontendContentPath))
            //{
            //    return AjaxResult.CreateError("配置文件中Application.ContentUrl为空或空字符");
            //}

            if (avatarFile.Length <= 0)
            {
                return AjaxResult.CreateError("文件大小为0");
            }

            if (avatarFile.Length > options.Avatar.GetMaxAvatarLength())
            {
                return AjaxResult.CreateError("文件大小超出最大值");
            }

            var (result, obj) = await _userCenterContract.UpdateAvatarAsync(User.Identity.Name, avatarFile);
            if (result)
            {
                ServiceProvider.GetService<IOnlineUserCache>()?.Remove(User.Identity.Name);
                return AjaxResult.CreateSuccess(obj);
            }
            else
                return AjaxResult.CreateError(obj);
        }


        /// <summary>
        /// 变更用户头像
        /// </summary>
        /// <param name="avatarFile">头像文件</param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("变更用户头像")]
        public async Task<AjaxResult> ChangeAvatar(IFormFile avatarFile)
        {
            Check.NotNull(avatarFile, nameof(avatarFile));
            var (result, msg, data) = await _userCenterContract.ChangeAvatarAsync(avatarFile, ServiceProvider);
            return new AjaxResult(msg, data, type: result ? AjaxResultType.Success : AjaxResultType.Error);
        }
    }
}
