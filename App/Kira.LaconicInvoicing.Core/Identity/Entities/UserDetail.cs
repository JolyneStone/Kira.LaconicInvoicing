using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using OSharp.Entity;


namespace Kira.LaconicInvoicing.Identity.Entities
{
    /// <summary>
    /// 实体类：用户详细信息
    /// </summary>
    [Description("用户详细信息")]
    public class UserDetail : EntityBase<int>
    {
        /// <summary>
        /// 获取或设置 注册IP
        /// </summary>
        [DisplayName("注册IP")]
        public string RegisterIp { get; set; }

        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        [DisplayName("用户编号")]
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 个人简介
        /// </summary>
        [DisplayName("Profile")]
        public string Profile { get; set; }

        /// <summary>
        /// 获取或设置 所属用户信息
        /// </summary>
        [JsonIgnore]
        public virtual User User { get; set; }


        ///// <summary>
        ///// 个人简介
        ///// </summary>
        //[DisplayName("个人简介")]
        //[NotMapped]
        //public string PersonalProfile { get; set; }

        ///// <summary>
        ///// 所在国家
        ///// </summary>
        //[DisplayName("所在国家")]
        //[NotMapped]
        //public string Country { get; set; }

        ///// <summary>
        ///// 所在省份
        ///// </summary>
        //[DisplayName("所在省份")]
        //[NotMapped]
        //public string Province { get; set; }

        ///// <summary>
        ///// 所在城市
        ///// </summary>
        //[DisplayName("所在城市")]
        //[NotMapped]
        //public string City { get; set; }

        ///// <summary>
        ///// 所在地址
        ///// </summary>
        //[DisplayName("所在地址")]
        //[NotMapped]
        //public string Address { get; set; }
    }
}