using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Kira.LaconicInvoicing.UserCenter.Dtos
{
    /// <summary>
    /// 输出DTO: 用户信息
    /// </summary>
    public class UserDetailOutputDto: IOutputDto
    {
        /// <summary>
        /// 唯一标识
        /// </summary>
        [DisplayName("标识")]
        public int Id { get; set; }

        /// <summary>
        /// 用户标识
        /// </summary>
        [DisplayName("用户标识")]
        public string UserId { get; set; }

        /// <summary>
        /// 用户账号
        /// </summary>
        [DisplayName("用户账号")]
        public string UserName { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [DisplayName("昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [DisplayName("邮箱")]
        public string Email { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DisplayName("联系电话")]
        public string Telephone { get; set; }

        /// <summary>
        /// 头像
        /// </summary>
        [DisplayName("头像")]
        public string Avatar { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        [DisplayName("角色")]
        public string[] Roles { get; set; }

        /// <summary>
        /// 获取或设置 个人简介
        /// </summary>
        [DisplayName("Profile")]
        public string Profile { get; set; }

        ///// <summary>
        ///// 个人简介
        ///// </summary>
        //[DisplayName("个人简介")]
        //public string PersonalProfile { get; set; }

        ///// <summary>
        ///// 所在国家
        ///// </summary>
        //[DisplayName("所在国家")]
        //public string Country { get; set; }

        ///// <summary>
        ///// 所在省份
        ///// </summary>
        //[DisplayName("所在省份")]
        //public string Province { get; set; }

        ///// <summary>
        ///// 所在城市
        ///// </summary>
        //[DisplayName("所在城市")]
        //public string City { get; set; }

        ///// <summary>
        ///// 所在地址
        ///// </summary>
        //[DisplayName("所在地址")]
        //public string Address { get; set; }
    }
}
