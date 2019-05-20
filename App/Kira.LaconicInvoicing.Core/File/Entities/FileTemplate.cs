using OSharp.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.File.Entities
{
    /// <summary>
    /// 实体类： 文档模板实体信息
    /// </summary>
    [Description("文档模板实体信息")]
    public class FileTemplate : EntityBase<Guid>, IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 模板名称
        /// </summary>
        [DisplayName("模板名称")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 模板路径
        /// </summary>
        [DisplayName("模板路径")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Path { get; set; }

        /// <summary>
        /// 获取或设置 备注
        /// </summary>
        [DisplayName("备注")]
        public string Comment { get; set; }

        /// <summary>
        /// 获取或设置 模板类型
        /// </summary>
        [DisplayName("模板类型")]
        public TemplateType Type { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        [DisplayName("最近更新时间")]
        public DateTime? DateTime { get; set; }
    }
}
