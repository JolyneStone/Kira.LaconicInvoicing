using Kira.LaconicInvoicing.Identity.Dtos;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Infrastructure.Options;
using Kira.LaconicInvoicing.UserCenter.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Identity;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.UserCenter
{
    /// <summary>
    /// 用户信息操作实现
    /// </summary>
    public class UserCenterService : IUserCenterContract
    {
        private readonly IRepository<UserDetail, int> _userDetailRepository;
        private readonly UserManager<User> _userManager;
        private readonly IRepository<User, int> _userRepository;
        private readonly ILogger<UserCenterService> _logger;

        /// <summary>
        /// 初始化一个<see cref="IdentityService"/>类型的新实例
        /// </summary>
        public UserCenterService(UserManager<User> userManager,
            ILoggerFactory loggerFactory,
            IRepository<User, int> userRepository,
            IRepository<UserDetail, int> userDetailRepository)
        {
            _userManager = userManager;
            _userRepository = userRepository;
            _logger = loggerFactory.CreateLogger<UserCenterService>();
            _userDetailRepository = userDetailRepository;
        }

        public async Task<UserDetailOutputDto> GetUserDetailAsync(string userName, IServiceProvider serviceProvider)
        {
            var user = await serviceProvider.GetService<IOnlineUserCache>()?.GetOrRefreshAsync(userName);
            if (user == null)
            {
                return null;
            }
            //IList<string> roles = await _userManager.GetRolesAsync();
            //bool isAdmin = _roleManager.Roles.Any(m => roles.Contains(m.Name) && m.IsAdmin);
            var userDetail = await _userDetailRepository.GetFirstAsync(ud => ud.UserId.ToString() == user.Id);
            var userDetailDto = user.MapTo<UserDetailOutputDto>().Map<UserDetailOutputDto>(userDetail);
            userDetailDto.Roles = user.Roles;
            return userDetailDto;
        }

        public async Task<ValueTuple<bool, string>> UpdateUserDetailAsync(UserDetailInputDto userDetailDto)
        {
            var userDetail = await _userDetailRepository.GetAsync(userDetailDto.Id);
            if (userDetail == null)
                return (false, "无法找到该用户的详细信息");

            var user = await _userRepository.GetAsync(userDetail.UserId);
            if (user == null)
                return (false, "无法找到该用户");

            await _userDetailRepository.UnitOfWork.BeginOrUseTransactionAsync();
            try
            {
                userDetail.Profile = userDetailDto.ToUserDetailJson();
                _userDetailRepository.Update(userDetail);
                user.NickName = userDetailDto.NickName;
                user.PhoneNumber = userDetailDto.Telephone;
                await _userRepository.UpdateAsync(user);
                _userDetailRepository.UnitOfWork.Commit();

                return (true, "更新成功");
            }
            catch (Exception ex)
            {
                _userDetailRepository.UnitOfWork.Rollback();
                var msg = $"更新用户:{user.UserName}信息失败:{ex.GetBaseException().Message}";
                _logger.LogError(msg);
                return (false, msg);
            }
        }

        public async Task<ValueTuple<bool, string, string>> ChangeAvatarAsync(IFormFile avatarFile, IServiceProvider service)
        {
            var msg = "";
            var options = service.GetRequiredService<IOptions<ApplicationOptions>>()?.Value;
            //if (string.IsNullOrWhiteSpace(options.FrontendContentPath))
            //{
            //    msg = "配置文件中Application.ContentUrl为空或空字符";
            //    _logger.LogWarning(msg);
            //    return (false, msg, "");
            //}

            if (avatarFile.Length <= 0)
            {
                msg = "文件大小为0";
                _logger.LogWarning(msg);
                return (false, msg, "");
            }

            if (avatarFile.Length > options.Avatar.GetMaxAvatarLength())
            {
                msg = "文件大小超出最大值";
                _logger.LogWarning(msg);
                return (false, msg, "");
            }

            try
            {
                //var directory = $"{options.FrontendContentPath}{Path.DirectorySeparatorChar}tmp";
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
                var directoryInfo = new DirectoryInfo(directory);
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
                var fileExtend = avatarFile.FileName.Substring(avatarFile.FileName.LastIndexOf('.'));
                var fileName = Guid.NewGuid().ToString() + fileExtend;
                var fileFullName = directory + Path.DirectorySeparatorChar + fileName;

                using (var stream = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                return (true, msg, fileName);
            }
            catch (Exception ex)
            {
                msg = $"保存临时头像数据失败:{ex.GetBaseException().Message}";
                _logger.LogError(msg);
                return (false, msg, "");
            }
        }

        public async Task<ValueTuple<bool, string>> UpdateAvatarAsync(string userName, IFormFile avatarFile)
        {
            var msg = "";
            await _userRepository.UnitOfWork.BeginOrUseTransactionAsync();
            try
            {
                var user = await _userManager.FindByNameAsync(userName);
                if(user == null)
                {
                    msg = $"找不到该用户:{userName}";
                    _logger.LogError(msg);
                    return (false, msg);
                }

                //var directory = $"{contenPath}{Path.DirectorySeparatorChar}avatars";
                var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "avatars");
                var directoryInfo = new DirectoryInfo(directory);
                if (!directoryInfo.Exists)
                    directoryInfo.Create();
                var fileExtend = avatarFile.FileName.Substring(avatarFile.FileName.LastIndexOf('.'));
                var fileName = Guid.NewGuid().ToString() + fileExtend;
                var fileFullName = directory + Path.DirectorySeparatorChar + fileName;

                var sourceAvatar = directory + Path.DirectorySeparatorChar + user.HeadImg;
                var sourceAvatarFile = new FileInfo(sourceAvatar);
                if (sourceAvatarFile.Exists)
                {
                    sourceAvatarFile.Delete();
                }

                using (var stream = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    await avatarFile.CopyToAsync(stream);
                }

                user.HeadImg = fileName;
                await _userManager.UpdateAsync(user);
                _userRepository.UnitOfWork.Commit();
                return (true, fileName);
            }
            catch (Exception ex)
            {
                msg = $"保存头像数据失败:{ex.GetBaseException().Message}";
                _logger.LogError(msg);
                _userRepository.UnitOfWork.Rollback();
                return (false, msg);
            }
        }
    }
}
