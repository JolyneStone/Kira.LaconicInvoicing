using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Print.Dtos
{
    public class PrintTemplateInputDto: IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 模板名称
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模板路径
        /// </summary>
        [StringLength(maximumLength: 255)]
        public string Path { get; set; }

        /// <summary>
        /// 获取或设置 模板代码
        /// </summary>
        [Required]
        public string Script { get; set; }

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
