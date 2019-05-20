using System;
using System.Threading.Tasks;

namespace OSharp.Identity
{
    /// <summary>
    /// 在线用户提供者
    /// </summary>
    public interface IOnlineUserProvider
    {
        /// <summary>
        /// 创建在线用户信息
        /// </summary>
        /// <param name="provider">服务提供器</param>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户信息</returns>
        Task<OnlineUser> Create(IServiceProvider provider, string userName);
    }
}