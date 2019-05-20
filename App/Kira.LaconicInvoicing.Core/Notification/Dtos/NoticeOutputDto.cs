using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Notification.Dtos
{
    public class NoticeOutputDto:IOutputDto
    {
        /// <summary>
        /// 获取或设置 Id
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 获取或设置 发布者
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// 获取或设置 通知内容
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 获取或设置 最近更新时间
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// 获取或设置 是否已读
        /// </summary>
        public bool IsRead { get; set; }
    }
}
