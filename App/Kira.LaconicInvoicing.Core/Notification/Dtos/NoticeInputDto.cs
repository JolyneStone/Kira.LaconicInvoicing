using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Notification.Dtos
{
    public class NoticeInputDto: IInputDto<Guid>
    {
        /// <summary>
        /// 获取或设置 Id
        /// </summary>
        [Required]
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 发布者
        /// </summary>
        [Required]
        [StringLength(maximumLength: 255)]
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置 通知内容
        /// </summary>
        [Required]
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }
    }
}
