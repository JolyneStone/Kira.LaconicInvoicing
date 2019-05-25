//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using OSharp.Entity;
//using OSharp.Identity;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Kira.LaconicInvoicing.Infrastructure.Cache
//{
//    /// <summary>
//    /// 在线用户详细信息提供者
//    /// </summary>
//    public class OnlineUserDetailProvider<TUserDetail, TUserDetailKey, TUser, TUserKey> : IOnlineUserDetailProvider
//        where TUserDetail : UserDetail<TUserDetailKey>
//        where TUserDetailKey : IEquatable<TUserDetailKey>
//        where TUser : UserBase<TUserKey>
//        where TUserKey : IEquatable<TUserKey>

//    {
//        /// <summary>
//        /// 创建在线用户详细信息
//        /// </summary>
//        /// <param name="provider">服务提供器</param>
//        /// <param name="userName">用户名</param>
//        /// <returns>在线用户详细信息</returns>
//        public virtual async Task<OnlineUserDetail> Create(IServiceProvider provider, string userName)
//        {
//            var user = await provider.GetService<IOnlineUserCache>()?.GetOrRefreshAsync(userName);
//            if (user == null)
//            {
//                return null;
//            }
//            //IList<string> roles = await _userManager.GetRolesAsync();
//            //bool isAdmin = _roleManager.Roles.Any(m => roles.Contains(m.Name) && m.IsAdmin);
//            var userDetail = await provider.GetService<IRepository<TUserDetail, TUserDetailKey>>().GetAsync(()user.Id).GetFirstAsync(ud => ud.UserId.ToString() == user.Id);
//            var userDetailDto = user.MapTo<UserDetailOutputDto>().Map<UserDetailOutputDto>(userDetail);
//            userDetailDto.Roles = user.Roles;
//            return userDetailDto;
//        }
//    }
//}
