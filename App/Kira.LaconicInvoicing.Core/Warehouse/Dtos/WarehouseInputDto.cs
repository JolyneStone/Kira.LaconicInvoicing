using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class WarehouseInputDto: IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 仓库Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        [Required]
        [StringLength(maximumLength: 18)]
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 仓库管理员
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Manager { get; set; }

        /// <summary>
        /// 获取或设置 仓库管理员联系方式
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string ManagerContact { get; set; }

        /// <summary>
        /// 获取或设置 仓库面积
        /// </summary>
        public double Area { get; set; }

        /// <summary>
        /// 获取或设置 仓库状态
        /// </summary>
        public WarehouseStatus Status { get; set; }

        /// <summary>
        /// 获取或设置 仓库地址
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// 获取或设置 仓库备注
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
