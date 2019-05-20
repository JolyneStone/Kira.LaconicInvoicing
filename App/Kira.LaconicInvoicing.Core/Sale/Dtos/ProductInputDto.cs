using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Sale.Dtos
{
    public class ProductInputDto: IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 产品Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 产品编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 产品名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 产品类型
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 规格
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Spec { get; set; }

        /// <summary>
        /// 获取或设置 品牌
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string Brand { get; set; }

        /// <summary>
        /// 获取或设置 单位
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Unit { get; set; }

        /// <summary>
        /// 获取或设置 成本价
        /// </summary>
        public double CostPrice { get; set; }

        /// <summary>
        /// 获取或设置 批发价
        /// </summary>
        public double WholesalePrice { get; set; }

        /// <summary>
        /// 获取或设置 零售价
        /// </summary>
        public double RetailPrice { get; set; }

        /// <summary>
        /// 获取或设置 备注
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
    }
}
