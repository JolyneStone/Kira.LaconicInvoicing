using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;

namespace Kira.LaconicInvoicing.EntityConfiguration.Purchase
{

    public class MaterialConfiguration : EntityTypeConfigurationBase<Material, Guid>
    {
        public override void Configure(EntityTypeBuilder<Material> builder)
        {
            builder.HasIndex(m => m.Number).IsUnique();
        }
    }
}
