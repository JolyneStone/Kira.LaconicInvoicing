using OSharp.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Purchase.Entities
{
    /// <summary>
    /// 采购单项目实体信息
    /// </summary>
    [Description("采购单项目实体信息")]
    public class PurchaseOrderItem: EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 采购单Id
        /// </summary>
        [DisplayName("采购单Id")]
        [Required]
        public Guid PurchaseOrderId { get; set; }

        /// <summary>
        /// 获取或设置 数量
        /// </summary>
        [DisplayName("数量")]
        [Required]
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 原料编号
        /// </summary>
        [DisplayName("原料编号")]
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
    }
}
