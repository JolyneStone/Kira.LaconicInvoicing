using Kira.LaconicInvoicing.UserCenter.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.UserCenter
{
    /// <summary>
    /// 业务契约：用户信息管理模块
    /// </summary>
    public interface IUserCenterContract
    {
        Task<UserDetailOutputDto> GetUserDetailAsync(string userName, IServiceProvider serviceProvider);
        Task<ValueTuple<bool, string>> UpdateUserDetailAsync(UserDetailInputDto userDetailDto);
        Task<ValueTuple<bool, string>> UpdateAvatarAsync(string userName, IFormFile avatarFile);
        Task<ValueTuple<bool, string, string>> ChangeAvatarAsync(IFormFile avatarFile, IServiceProvider service);
    }
}
