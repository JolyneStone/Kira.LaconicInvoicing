using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Purchase.Dtos
{
    public class MaterialInputDto: IInputDto<Guid>
    {
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 原料编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 原料名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 原料类型
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 规格
        /// </summary>
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
        [StringLength(maximumLength: 255)]
        public string Unit { get; set; }

        ///// <summary>
        ///// 获取或设置 成本价
        ///// </summary>
        //[Range(0, double.MaxValue)]
        //public double? CostPrice { get; set; }

        ///// <summary>
        ///// 获取或设置 批发价
        ///// </summary>
        //[Range(0, double.MaxValue)]
        //public double? WholesalePrice { get; set; }

        ///// <summary>
        ///// 获取或设置 零售价
        ///// </summary>
        //[Range(0, double.MaxValue)]
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
        [StringLength(maximumLength: 255)]
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 获取或设置 供应商ID
        /// </summary>
        [Required]
        public Guid VendorId { get; set; }
    }
}
