using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Entities
{
    /// <summary>
    /// 仓库实体信息
    /// </summary>
    [Description("仓库实体信息")]
    public class Warehouse: EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        [DisplayName("仓库编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        [DisplayName("仓库名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 仓库管理员
        /// </summary>
        [DisplayName("仓库管理员")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Manager { get; set; }

        /// <summary>
        /// 获取或设置 仓库管理员联系方式
        /// </summary>
        [DisplayName("仓库管理员联系方式")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string ManagerContact { get; set; }

        /// <summary>
        /// 获取或设置 仓库面积
        /// </summary>
        [DisplayName("仓库面积")]
        public double Area { get; set; }

        /// <summary>
        /// 获取或设置 仓库状态
        /// </summary>
        [DisplayName("仓库状态")]
        public WarehouseStatus Status { get; set; }

        /// <summary>
        /// 获取或设置 仓库地址
        /// </summary>
        [DisplayName("仓库地址")]
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 仓库备注
        /// </summary>
        [DisplayName("仓库备注")]
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

        //public virtual List<Inventory> Inventories { get; set; }
    }
}
