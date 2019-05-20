using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Sale.Dtos
{
    public class CustomerOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 客户Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 客户编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 客户名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置客户类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 应收欠款
        /// </summary>
        public double Debt { get; set; }

        /// <summary>
        /// 获取或设置 联系人
        /// </summary>
        public string ContactPerson { get; set; }

        /// <summary>
        /// 获取或设置 联系电话
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 客户地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 客户备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
