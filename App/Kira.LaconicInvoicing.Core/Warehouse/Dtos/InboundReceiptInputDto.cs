using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class InboundReceiptInputDto : IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 入库单Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string WarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string WarehouseName { get; set; }

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
        /// 获取或设置 供应方
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Supplier { get; set; }

        /// <summary>
        /// 获取或设置 供应方编号
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string SupplierNo { get; set; }

        /// <summary>
        /// 获取或设置 供应地址
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string SupplyAddress { get; set; }

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
        /// 获取或设置 入库单备注
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
        /// 获取或设置 入库单项目
        /// </summary>
        [Required]
        public InboundReceiptItemInputDto[] Items { get; set; }
    }
}
