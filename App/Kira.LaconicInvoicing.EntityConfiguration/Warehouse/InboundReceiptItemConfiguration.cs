using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Warehouse
{
    public class InboundReceiptItemConfiguration : EntityTypeConfigurationBase<InboundReceiptItem, Guid>
    {
        public override void Configure(EntityTypeBuilder<InboundReceiptItem> builder)
        {
        }
    }
}
