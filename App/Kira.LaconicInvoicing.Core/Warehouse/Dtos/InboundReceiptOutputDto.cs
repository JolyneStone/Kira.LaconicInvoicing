using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class InboundReceiptOutputDto : IOutputDto
    {
        /// <summary>
        /// 获取或设置 入库单Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        public string WarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 运输单
        /// </summary>
        public string WaybillNo { get; set; }

        /// <summary>
        /// 获取或设置 供应方
        /// </summary>
        public string Supplier { get; set; }

        /// <summary>
        /// 获取或设置 供应方编号
        /// </summary>
        public string SupplierNo { get; set; }

        /// <summary>
        /// 获取或设置 供应地址
        /// </summary>
        public string SupplyAddress { get; set; }

        /// <summary>
        /// 获取或设置 运输方
        /// </summary>
        public string DeliveryMan { get; set; }

        /// <summary>
        /// 获取或设置 运输方联系方式
        /// </summary>
        public string DeliveryManContact { get; set; }

        /// <summary>
        /// 获取或设置 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 获取或设置 收货人联系方式
        /// </summary>
        public string ConsigneeContact { get; set; }

        /// <summary>
        /// 获取或设置 入库单备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置 操作员
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 获取或设置 入库单项目
        /// </summary>
        public InboundReceiptItemOutputDto[] Items { get; set; }
    }
}
