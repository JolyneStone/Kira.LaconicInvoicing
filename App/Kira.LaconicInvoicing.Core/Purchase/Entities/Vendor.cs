using System.ComponentModel.DataAnnotations;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.ComponentModel.DataAnnotations.Schema;

namespace Kira.LaconicInvoicing.Purchase.Entities
{
    /// <summary>
    /// 实体类： 供应商实体信息
    /// </summary>
    [Description("供应商实体信息")]
    public class Vendor : EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 供应商编号
        /// </summary>
        [DisplayName("供应商编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 供应商名称
        /// </summary>
        [DisplayName("供应商名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置供应商类型
        /// </summary>
        [DisplayName("供应商类型")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 应付欠款
        /// </summary>
        [DisplayName("应付欠款")]
        [DefaultValue(0.0)]
        public double Debt { get; set; }

        /// <summary>
        /// 获取或设置 联系人
        /// </summary>
        [DisplayName("联系人")]
        [StringLength(maximumLength: 255)]
        public string ContactPerson { get; set; }

        /// <summary>
        /// 获取或设置 联系电话
        /// </summary>
        [DisplayName("联系电话")]
        [StringLength(maximumLength: 20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [DisplayName("电子邮箱")]
        [StringLength(maximumLength: 255)]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 供应商地址
        /// </summary>
        [DisplayName("供应商地址")]
        [StringLength(maximumLength: 255)]
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 供应商备注
        /// </summary>
        [DisplayName("供应商备注")]
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置 操作员
        /// </summary>
        [DisplayName("操作员")]
        [StringLength(maximumLength: 255)]
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        [DisplayName("最近更新时间")]
        public DateTime? DateTime { get; set; }
    }
}
