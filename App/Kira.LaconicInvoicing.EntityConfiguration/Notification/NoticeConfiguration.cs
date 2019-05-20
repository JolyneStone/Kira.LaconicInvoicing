using Kira.LaconicInvoicing.Notification.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Notification
{
    public class NoticeConfiguration : EntityTypeConfigurationBase<Notice, Guid>
    {
        public override void Configure(EntityTypeBuilder<Notice> builder)
        {
        }
    }
}
