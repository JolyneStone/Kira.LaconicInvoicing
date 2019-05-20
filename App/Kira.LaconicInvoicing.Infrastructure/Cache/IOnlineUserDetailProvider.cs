using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Infrastructure.Cache
{
    /// <summary>
    /// 在线用户详细信息提供者
    /// </summary>
    public interface IOnlineUserDetailProvider
    {
        /// <summary>
        /// 创建在线用户详细信息
        /// </summary>
        /// <param name="provider">服务提供其</param>
        /// <param name="userName">用户名</param>
        /// <returns>在线用户详细信息</returns>
        Task<OnlineUserDetail> Create(IServiceProvider provider, string userName);
    }
}
