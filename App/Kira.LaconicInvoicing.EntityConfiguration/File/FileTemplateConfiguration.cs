using Kira.LaconicInvoicing.Notification.Entities;
using Kira.LaconicInvoicing.Print.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Print
{
    public class PrintTemplateConfiguration : EntityTypeConfigurationBase<PrintTemplate, Guid>
    {
        public override void Configure(EntityTypeBuilder<PrintTemplate> builder)
        {
        }
    }
}
