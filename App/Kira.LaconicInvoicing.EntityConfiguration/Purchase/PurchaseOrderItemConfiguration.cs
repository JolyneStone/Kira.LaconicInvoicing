using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Purchase
{
    public class PurchaseOrderItemConfiguration : EntityTypeConfigurationBase<PurchaseOrderItem, Guid>
    {
        public override void Configure(EntityTypeBuilder<PurchaseOrderItem> builder)
        {
        }
    }
}
