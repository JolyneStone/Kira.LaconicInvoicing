using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.File.Dtos
{
    public class FileTemplateOutputDto: IOutputDto
    {
        /// <summary>
        /// 获取或设置 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 模板名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模板路径
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置 模板类型
        /// </summary>
        public TemplateType Type { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
