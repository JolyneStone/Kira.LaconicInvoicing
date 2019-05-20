using OSharp.Entity;
using System;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class WarehouseOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 仓库Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 仓库编号
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// 获取或设置 仓库名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 仓库管理员
        /// </summary>
        public string Manager { get; set; }

        /// <summary>
        /// 获取或设置 仓库管理员联系方式
        /// </summary>
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
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
