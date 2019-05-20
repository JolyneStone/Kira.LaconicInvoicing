using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Entities
{
    /// <summary>
    /// 库存实体信息
    /// </summary>
    [Description("库存实体信息")]
    public class Inventory: EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 库存数量
        /// </summary>
        [DisplayName("库存数量")]
        [Required]
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 仓库Id
        /// </summary>
        [DisplayName("仓库Id")]
        [Required]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// 获取或设置 库存项编号
        /// </summary>
        [DisplayName("库存项编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 库存项名称
        /// </summary>
        [DisplayName("库存项名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 货物分类
        /// </summary>
        [DisplayName("货物分类")]
        [Required]
        public GoodsCategory GoodsCategory { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        [DisplayName("最近更新时间")]
        public DateTime? DateTime { get; set; }
    }
}
