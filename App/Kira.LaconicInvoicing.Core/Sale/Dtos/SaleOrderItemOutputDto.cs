using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Sale.Dtos
{
    public class SaleOrderItemOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 销售单项目Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 销售单Id
        /// </summary>
        public Guid SaleOrderId { get; set; }

        /// <summary>
        /// 获取或设置 数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 产品编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 产品名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 产品类型
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
