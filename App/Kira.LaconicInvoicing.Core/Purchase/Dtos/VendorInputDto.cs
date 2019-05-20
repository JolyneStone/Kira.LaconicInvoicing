using OSharp.Entity;
using System;
using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Purchase.Dtos
{
    public class VendorInputDto: IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 供应商Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }
        
        /// <summary>
        /// 获取或设置 供应商编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 供应商名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置供应商类型
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 应付欠款
        /// </summary>
        [Range(0, double.MaxValue)]
        public double Debt { get; set; }

        /// <summary>
        /// 获取或设置 联系人
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string ContactPerson { get; set; }

        /// <summary>
        /// 获取或设置 联系电话
        /// </summary>
        [StringLength(maximumLength: 20)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 供应商地址
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 供应商备注
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
