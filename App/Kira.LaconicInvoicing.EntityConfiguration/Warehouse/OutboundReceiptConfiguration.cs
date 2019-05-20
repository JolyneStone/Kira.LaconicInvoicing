using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Warehouse
{
    public class OutboundReceiptConfiguration : EntityTypeConfigurationBase<OutboundReceipt, Guid>
    {
        public override void Configure(EntityTypeBuilder<OutboundReceipt> builder)
        {
            builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}
