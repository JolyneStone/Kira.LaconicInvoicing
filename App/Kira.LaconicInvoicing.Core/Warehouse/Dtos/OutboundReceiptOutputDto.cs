using OSharp.Entity;
using System;
using System.ComponentModel;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class OutboundReceiptOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 出库单Id
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
        /// 获取或设置 接收方
        /// </summary>
        public string Receiver { get; set; }

        /// <summary>
        /// 获取或设置 接收方编号
        /// </summary>
        public string ReceiverNo { get; set; }

        /// <summary>
        /// 获取或设置 收货地址
        /// </summary>
        public string ReceiverAddress { get; set; }

        /// <summary>
        /// 获取或设置 运输方
        /// </summary>
        public string DeliveryMan { get; set; }

        /// <summary>
        /// 获取或设置 运输方联系方式
        /// </summary>
        public string DeliveryManContact { get; set; }

        /// <summary>
        /// 获取或设置 发货人
        /// </summary>
        public string Consignor { get; set; }

        /// <summary>
        /// 获取或设置 发货人联系方式
        /// </summary>
        public string ConsignorContact { get; set; }

        /// <summary>
        /// 获取或设置 出库单备注
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
        /// 出库单项目
        /// </summary>
        public OutboundReceiptItemOutputDto[] Items { get; set; }
    }
}