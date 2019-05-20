using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace Kira.LaconicInvoicing.UserCenter.Dtos
{
    /// <summary>
    /// 更新用户详细信息DTO
    /// </summary>
    public class UserDetailInputDto: IInputDto<int>
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
        public int UserId { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [DisplayName("昵称")]
        public string NickName { get; set; }

        /// <summary>
        /// 联系电话
        /// </summary>
        [DisplayName("联系电话")]
        public string Telephone { get; set; }

        /// <summary>
        /// 个人简介
        /// </summary>
        [DisplayName("个人简介")]
        public string PersonalProfile { get; set; }

        /// <summary>
        /// 所在国家
        /// </summary>
        [DisplayName("所在国家")]
        public string Country { get; set; }

        /// <summary>
        /// 所在省份
        /// </summary>
        [DisplayName("所在省份")]
        public string Province { get; set; }

        /// <summary>
        /// 所在城市
        /// </summary>
        [DisplayName("所在城市")]
        public string City { get; set; }

        /// <summary>
        /// 所在地址
        /// </summary>
        [DisplayName("所在地址")]
        public string Address { get; set; }

        /// <summary>
        /// 部门
        /// </summary>
        /// <returns></returns>
        [DisplayName("部门")]
        public string Department { get; set; }

        public string ToUserDetailJson()
        {
            var jObject = new JObject
            {
                { "personalProfile", PersonalProfile },
                { "country", Country },
                { "province", Province },
                { "city", City },
                { "address", Address },
                { "department", Department }
            };

            return jObject.ToString();
        }
    }
}
