using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class TransferOrderInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 调拨单Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 调出仓库编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string SourceWarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 调出仓库名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string SourceWarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 源地址
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string SourceAddress { get; set; }

        /// <summary>
        /// 获取或设置 调入仓库编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string DestWarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 调入仓库名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string DestWarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 目标地址
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string DestAddress { get; set; }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 运输单
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string WaybillNo { get; set; }

        /// <summary>
        /// 获取或设置 运输方
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string DeliveryMan { get; set; }

        /// <summary>
        /// 获取或设置 运输方联系方式
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string DeliveryManContact { get; set; }

        /// <summary>
        /// 获取或设置 收货人
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Consignee { get; set; }

        /// <summary>
        /// 获取或设置 收货人联系方式
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string ConsigneeContact { get; set; }

        /// <summary>
        /// 获取或设置 发货人
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Consignor { get; set; }

        /// <summary>
        /// 获取或设置 发货人联系方式
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string ConsignorContact { get; set; }

        /// <summary>
        /// 获取或设置 调拨单备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置 操作员
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 获取或设置 调拨单项目
        /// </summary>
        [Required]
        public TransferOrderItemInputDto[] Items { get; set; }
    }
}
