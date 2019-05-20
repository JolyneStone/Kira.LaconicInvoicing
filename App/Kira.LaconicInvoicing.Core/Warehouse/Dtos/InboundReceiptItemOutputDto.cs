using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class InboundReceiptItemOutputDto : IOutputDto
    {
        /// <summary>
        /// 获取或设置 入库单项目Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 入库单Id
        /// </summary>
        public Guid InboundReceiptId { get; set; }

        /// <summary>
        /// 获取或设置 货物分类
        /// </summary>
        public GoodsCategory GoodsCategory { get; set; }

        /// <summary>
        /// 获取或设置 数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 货物编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 货物名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 货物类型
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
