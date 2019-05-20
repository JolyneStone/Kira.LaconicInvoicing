using Kira.LaconicInvoicing.Purchase;
using Kira.LaconicInvoicing.Purchase.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Purchase.Dtos
{

    public class PurchaseOrderPrintDataDto
    {
        /// <summary>
        /// 获取或设置 采购单编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 供应商编号
        /// </summary>
        public string VendorNumber { get; set; }

        /// <summary>
        /// 获取或设置 供应商名称
        /// </summary>
        public string VendorName { get; set; }

        /// <summary>
        /// 获取或设置 发货人
        /// </summary>
        public string Consignor { get; set; }

        /// <summary>
        /// 获取或设置 发货人联系方式
        /// </summary>
        public string ConsignorContact { get; set; }

        /// <summary>
        /// 获取或设置 发货地址
        /// </summary>
        public string SourceAddress { get; set; }

        /// <summary>
        /// 获取或设置 收获
        /// </summary>
        public string DestAddress { get; set; }

        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        public string WarehouseNumber { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        public string WarehouseName { get; set; }

        /// <summary>
        /// 获取或设置 收货人
        /// </summary>
        public string Consignee { get; set; }

        /// <summary>
        /// 获取或设置 收货人联系方式
        /// </summary>
        public string ConsigneeContact { get; set; }

        /// <summary>
        /// 获取或设置 运费
        /// </summary>
        public double? Freight { get; set; }

        /// <summary>
        /// 获取或设置 合计金额
        /// </summary>
        public double TotalAmount { get; set; }

        /// <summary>
        /// 获取或设置 合计数量
        /// </summary>
        public int TotalQuantity { get; set; }

        /// <summary>
        /// 获取或设置 实付金额
        /// </summary>
        public double AmountPaid { get; set; }

        /// <summary>
        /// 获取或设置 支付金额
        /// </summary>
        public string PayWay { get; set; }

        /// <summary>
        /// 获取或设置 采购单状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 获取或设置 采购单备注
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
        /// 获取或设置 采购单项目
        /// </summary>
        public PurchaseOrderItemOutputDto[] Items { get; set; }
    }
}
