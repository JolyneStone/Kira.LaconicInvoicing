using OSharp.Entity;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Common.Entities
{
    /// <summary>
    /// 实体类：数据字典实体
    /// </summary>
    [Description("数据字典实体")]
    public class BaseData : EntityBase<Guid>
    {
        /// <summary>
        /// 获取或设置 数据类型
        /// </summary>
        [DisplayName("数据类型")]
        [StringLength(maximumLength: 255)]
        public string Type { get; set; }

        /// <summary>
        /// 获取或设置 数据字典值
        /// </summary>
        [DisplayName("数据字典值")]
        public string Value { get; set; }
    }
}
