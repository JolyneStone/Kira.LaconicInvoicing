using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Entities
{
    /// <summary>
    /// 出库单实体信息
    /// </summary>
    [Description("出库单实体信息")]
    public class OutboundReceipt : EntityBase<Guid>, IDateTimeEntity
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
        /// 获取或设置 接收方
        /// </summary>
        [DisplayName("接收方")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Receiver { get; set; }

        /// <summary>
        /// 获取或设置 接收方编号
        /// </summary>
        [DisplayName("接收方编号")]
        [StringLength(maximumLength: 255)]
        public string ReceiverNo { get; set; }

        /// <summary>
        /// 获取或设置 收货地址
        /// </summary>
        [DisplayName("收货地址")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string ReceiverAddress { get; set; }

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
        /// 获取或设置 发货人
        /// </summary>
        [DisplayName("发货人")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Consignor { get; set; }

        /// <summary>
        /// 获取或设置 发货人联系方式
        /// </summary>
        [DisplayName("发货人联系方式")]
        [StringLength(maximumLength: 255)]
        public string ConsignorContact { get; set; }

        /// <summary>
        /// 获取或设置 出库单备注
        /// </summary>
        [DisplayName("出库单备注")]
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
