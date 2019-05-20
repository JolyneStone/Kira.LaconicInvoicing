using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Purchase.Dtos
{
    public class MaterialOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 原料Id
        /// </summary>
        public Guid Id { get; set; }

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

        ///// <summary>
        ///// 获取或设置 成本价
        ///// </summary>
        //public double? CostPrice { get; set; }

        ///// <summary>
        ///// 获取或设置 批发价
        ///// </summary>
        //public double? WholesalePrice { get; set; }

        ///// <summary>
        ///// 获取或设置 零售价
        ///// </summary>
        //public double? RetailPrice { get; set; }


        /// <summary>
        /// 获取或设置 单价
        /// </summary>
        public double Price { get; set; }

        /// <summary>
        /// 获取或设置 备注
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
    }
}
