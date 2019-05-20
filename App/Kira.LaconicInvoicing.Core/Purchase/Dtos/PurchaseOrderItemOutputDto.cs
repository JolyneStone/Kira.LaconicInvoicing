using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Purchase.Dtos
{
    public class PurchaseOrderItemOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 采购单项目Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 原料编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 原料名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 原料类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 获取或设置 品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 获取或设置 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 获取或设置 单价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        public string Comment { get; set; }
    }
}
