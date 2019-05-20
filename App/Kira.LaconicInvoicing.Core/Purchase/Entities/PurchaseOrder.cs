using Kira.LaconicInvoicing.Purchase.Dtos;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Purchase.Entities
{
    /// <summary>
    /// 采购单实体信息
    /// </summary>
    [Description("采购单实体信息")]
    public class PurchaseOrder : EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 采购单编号
        /// </summary>
        [DisplayName("采购单编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 供应商编号
        /// </summary>
        [DisplayName("供应商编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string VendorNumber { get; set; }

        /// <summary>
        /// 获取或设置 供应商名称
        /// </summary>
        [Required]
        [DisplayName("供应商名称")]
        [StringLength(maximumLength: 255)]
        public string VendorName { get; set; }

        /// <summary>
        /// 获取或设置 发货人联系方式
        /// </summary>
        [Required]
        [DisplayName("发货人联系方式")]
        [StringLength(maximumLength: 255)]
        public string ConsignorContact { get; set; }

        /// <summary>
        /// 获取或设置 发货人
        /// </summary>
        [DisplayName("发货人")]
        [StringLength(maximumLength: 255)]
        public string Consignor { get; set; }

        /// <summary>
        /// 获取或设置 发货地址
        /// </summary>
        [Required]
        [DisplayName("发货地址")]
        [StringLength(maximumLength: 255)]
        public string SourceAddress { get; set; }

        /// <summary>
        /// 获取或设置 收获
        /// </summary>
        [Required]
        [DisplayName("收货地址")]
        [StringLength(maximumLength: 255)]
        public string DestAddress { get; set; }

        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        [Required]
        [DisplayName("仓库编号")]
        [StringLength(maximumLength: 255)]
        public string WarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        [Required]
        [DisplayName("仓库名称")]
        [StringLength(maximumLength: 255)]
        public string WarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 收货人
        /// </summary>
        [DisplayName("收货人")]
        [StringLength(maximumLength: 255)]
        public string Consignee { get; set; }

        /// <summary>
        /// 获取或设置 收货人联系方式
        /// </summary>
        [Required]
        [DisplayName("收货人联系方式")]
        [StringLength(maximumLength: 255)]
        public string ConsigneeContact { get; set; }

        /// <summary>
        /// 获取或设置 运费
        /// </summary>
        [DisplayName("运费")]
        public double? Freight { get; set; }

        /// <summary>
        /// 获取或设置 合计金额
        /// </summary>
        [DisplayName("合计金额")]
        public double TotalAmount { get; set; }

        /// <summary>
        /// 获取或设置 合计数量
        /// </summary>
        [DisplayName("合计数量")]
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 获取或设置 实付金额
        /// </summary>
        [DisplayName("实付金额")]
        public double AmountPaid { get; set; }

        /// <summary>
        /// 获取或设置 支付金额
        /// </summary>
        [DisplayName("支付方式")]
        public string PayWay { get; set; }

        /// <summary>
        /// 获取或设置 采购单状态
        /// </summary>
        [DisplayName("采购单状态")]
        public PurchaseOrderStatus Status { get; set; }

        /// <summary>
        /// 获取或设置 采购单备注
        /// </summary>
        [DisplayName("采购单备注")]
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
