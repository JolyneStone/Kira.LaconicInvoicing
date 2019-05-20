using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Entities
{
    /// <summary>
    /// 调拨单实体信息
    /// </summary>
    [Description("调拨单实体信息")]
    public class TransferOrder : EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 调出仓库编号
        /// </summary>
        [DisplayName("调出仓库编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string SourceWarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 调出仓库名称
        /// </summary>
        [DisplayName("调出仓库名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string SourceWarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 源地址
        /// </summary>
        [DisplayName("源地址")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string SourceAddress { get; set; }

        /// <summary>
        /// 获取或设置 调入仓库编号
        /// </summary>
        [DisplayName("调入仓库编号")]
        [Required]
        [StringLength(maximumLength: 18)]
        public string DestWarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 调入仓库名称
        /// </summary>
        [DisplayName("调入仓库名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string DestWarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 目标地址
        /// </summary>
        [DisplayName("目标地址")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string DestAddress { get; set; }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [DisplayName("编号")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Number { get; set; }

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
        /// 获取或设置 运输单号
        /// </summary>
        [DisplayName("运输单号")]
        [StringLength(maximumLength: 255)]
        public string WaybillNo { get; set; }

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
        /// 获取或设置 调拨单备注
        /// </summary>
        [DisplayName("调拨单备注")]
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
