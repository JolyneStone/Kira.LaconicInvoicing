using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class InventoryOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 库存Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 库存数量
        /// </summary>
        public int Amount { get; set; }

        /// <summary>
        /// 获取或设置 仓库Id
        /// </summary>
        public Guid WarehouseId { get; set; }

        /// <summary>
        /// 获取或设置 项目编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 规格
        /// </summary>
        public string Spec { get; set; }

        /// <summary>
        /// 获取或设置 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 价格
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 获取或设置 品牌
        /// </summary>
        public string Brand { get; set; }

        /// <summary>
        /// 获取或设置 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 获取或设置 货物分类
        /// </summary>
        public GoodsCategory GoodsCategory { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
