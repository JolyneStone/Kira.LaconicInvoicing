using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Infrastructure.Cache
{
    /// <summary>
    /// 在线用户详细信息
    /// </summary>
    public class OnlineUserDetail
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        public string Telephone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public string[] Roles { get; set; }

        /// <summary>
        /// 获取或设置 个人简介
        /// </summary>
        public string Profile { get; set; }
    }
}
