using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Entities
{
    /// <summary>
    /// 入库单实体信息
    /// </summary>
    [Description("入库单实体信息")]
    public class InboundReceipt : EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        [DisplayName("仓库编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string WarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        [DisplayName("仓库名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 运输单
        /// </summary>
        [DisplayName("运输单")]
        [StringLength(maximumLength: 255)]
        public string WaybillNo { get; set; }

        /// <summary>
        /// 获取或设置 供应方
        /// </summary>
        [DisplayName("供应方")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Supplier { get; set; }

        /// <summary>
        /// 获取或设置 供应方编号
        /// </summary>
        [DisplayName("供应方编号")]
        [StringLength(maximumLength: 255)]
        public string SupplierNo { get; set; }

        /// <summary>
        /// 获取或设置 供应地址
        /// </summary>
        [DisplayName("供应地址")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string SupplyAddress { get; set; }

        /// <summary>
        /// 获取或设置 运输方
        /// </summary>
        [DisplayName("运输方")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string DeliveryMan { get; set; }

        /// <summary>
        /// 获取或设置 运输方联系方式
        /// </summary>
        [DisplayName("运输方联系方式")]
        [StringLength(maximumLength: 255)]
        public string DeliveryManContact { get; set; }

        /// <summary>
        /// 获取或设置 收货人
        /// </summary>
        [DisplayName("收货人")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Consignee { get; set; }

        /// <summary>
        /// 获取或设置 收货人联系方式
        /// </summary>
        [DisplayName("收货人联系方式")]
        [StringLength(maximumLength: 255)]
        public string ConsigneeContact { get; set; }

        /// <summary>
        /// 获取或设置 入库单备注
        /// </summary>
        [DisplayName("入库单备注")]
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
