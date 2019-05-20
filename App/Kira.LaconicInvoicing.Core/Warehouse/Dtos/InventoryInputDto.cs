using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class InventoryInputDto: IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 库存Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 库存数量
        /// </summary>
        [Required]
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 仓库Id
        /// </summary>
        [Required]
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// 获取或设置 库存项编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 库存项编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 货物分类
        /// </summary>
        [Required]
        public GoodsCategory GoodsCategory { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
