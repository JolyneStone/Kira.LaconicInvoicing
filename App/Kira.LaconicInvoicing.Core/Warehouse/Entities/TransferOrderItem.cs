using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Entities
{
    /// <summary>
    /// 调拨项目实体信息
    /// </summary>
    [Description("调拨单项目实体信息")]
    public class TransferOrderItem : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 调拨单Id
        /// </summary>
        [DisplayName("调拨单Id")]
        [Required]
        public Guid TransferOrderId { get; set; }

        /// <summary>
        /// 获取或设置 货物分类
        /// </summary>
        [DisplayName("货物分类")]
        [Required]
        public GoodsCategory GoodsCategory { get; set; }

        /// <summary>
        /// 获取或设置 数量
        /// </summary>
        [DisplayName("数量")]
        [Required]
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 货物编号
        /// </summary>
        [DisplayName("货物编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 货物名称
        /// </summary>
        [DisplayName("货物名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 货物类型
        /// </summary>
        [DisplayName("货物类型")]
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
