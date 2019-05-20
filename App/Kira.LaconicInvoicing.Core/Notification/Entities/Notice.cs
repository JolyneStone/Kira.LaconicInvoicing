using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Notification.Entities
{
    /// <summary>
    /// 实体类： 通知实体信息
    /// </summary>
    [Description("通知实体信息")]
    public class Notice: EntityBase<Guid>,IDateTimeEntity
    {
        /// <summary>
        /// 获取或设置 发布者
        /// </summary>
        [DisplayName("发布者")]
        [Required]
        [StringLength(maximumLength: 255)]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置 通知内容
        /// </summary>
        [DisplayName("通知内容")]
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        [DisplayName("最近更新时间")]
        public DateTime? DateTime { get; set; }
    }
}
