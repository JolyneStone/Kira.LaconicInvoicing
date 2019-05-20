using Kira.LaconicInvoicing.Sale.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.Sale
{
    public class CustomerConfiguration : EntityTypeConfigurationBase<Customer, Guid>
    {
        public override void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}
