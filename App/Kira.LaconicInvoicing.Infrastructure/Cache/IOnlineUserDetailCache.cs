using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Infrastructure.Cache
{
    /// <summary>
    /// 定义在线用户详细信息缓存
    /// </summary>
    public interface IOnlineUserDetailCache
    {
        /// <summary>
        /// 获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        OnlineUserDetail GetOrRefresh(string userName);

        /// <summary>
        /// 异步获取或刷新在线用户信息
        /// </summary>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        Task<OnlineUserDetail> GetOrRefreshAsync(string userName);

        /// <summary>
        /// 移除在线用户信息
        /// </summary>
        /// <param name="userNames">用户名</param>
        /// <returns>移除的用户信息</returns>
        void Remove(params string[] userNames);
    }
}
