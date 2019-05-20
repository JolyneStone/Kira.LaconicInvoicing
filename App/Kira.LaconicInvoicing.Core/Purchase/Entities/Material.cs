using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Purchase.Entities
{
    /// <summary>
    /// 实体类： 原料实体信息
    /// </summary>
    [Description("原料实体信息")]
    public class Material : EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 原料编号
        /// </summary>
        [DisplayName("编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 原料名称
        /// </summary>
        [DisplayName("原料名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 原料类型
        /// </summary>
        [DisplayName("原料类型")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 规格
        /// </summary>
        [DisplayName("规格")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Spec { get; set; }

        /// <summary>
        /// 获取或设置 品牌
        /// </summary>
        [DisplayName("品牌")]
        [StringLength(maximumLength: 255)]
        public string Brand { get; set; }

        /// <summary>
        /// 获取或设置 单位
        /// </summary>
        [DisplayName("单位")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Unit { get; set; }

        ///// <summary>
        ///// 获取或设置 成本价
        ///// </summary>
        //[DisplayName("成本价")]
        //public double? CostPrice { get; set; }

        ///// <summary>
        ///// 获取或设置 批发价
        ///// </summary>
        //[DisplayName("批发价")]
        //public double? WholesalePrice { get; set; }

        ///// <summary>
        ///// 获取或设置 零售价
        ///// </summary>
        //[DisplayName("零售价")]
        //public double? RetailPrice { get; set; }

        /// <summary>
        /// 获取或设置 单价
        /// </summary>
        [DisplayName("单价")]
        public double Price { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        [DisplayName("备注")]
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
