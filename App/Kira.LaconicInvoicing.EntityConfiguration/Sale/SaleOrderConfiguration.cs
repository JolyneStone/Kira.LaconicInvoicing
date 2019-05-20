using Kira.LaconicInvoicing.Sale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Sale
{
    public class SaleOrderConfiguration : EntityTypeConfigurationBase<SaleOrder, Guid>
    {
        public override void Configure(EntityTypeBuilder<SaleOrder> builder)
        {
            builder.HasIndex(b => b.Number).IsUnique();
        }
    }
}
