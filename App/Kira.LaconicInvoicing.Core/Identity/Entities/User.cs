using OSharp.Identity;
using System.Collections.Generic;
using System.ComponentModel;
using Newtonsoft.Json;

namespace Kira.LaconicInvoicing.Identity.Entities
{
    /// <summary>
    /// 实体类：用户信息
    /// </summary>
    [Description("用户信息")]
    public class User : UserBase<int>
    {
        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        [DisplayName("备注")]
        public string Remark { get; set; }

        /// <summary>
        /// 获取或设置 用户详细信息
        /// </summary>
        [JsonIgnore]
        public virtual UserDetail UserDetail { get; set; }

        /// <summary>
        /// 获取或设置 分配的用户角色信息集合
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();

        /// <summary>
        /// 获取或设置 用户的声明信息集合
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<UserClaim> UserClaims { get; set; } = new List<UserClaim>();

        /// <summary>
        /// 获取或设置 用户的第三方登录信息集合
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<UserLogin> UserLogins { get; set; } = new List<UserLogin>();

        /// <summary>
        /// 获取或设置 用户令牌信息集合
        /// </summary>
        [JsonIgnore]
        public virtual ICollection<UserToken> UserTokens { get; set; } = new List<UserToken>();
    }
}