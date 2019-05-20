using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Notification.Entities
{
    /// <summary>
    /// 实体类： 通知接受情况实体信息
    /// </summary>
    [Description("通知接受情况实体信息")]
    public class NoticeReceiving : EntityBase<Guid>
    {
        [Required]
        public Guid NoticeId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public bool IsRead { get; set; }
    }
}
