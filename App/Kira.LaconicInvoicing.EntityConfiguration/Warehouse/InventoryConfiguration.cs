using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;

namespace Kira.LaconicInvoicing.EntityConfiguration.Warehouse
{
    public class InventoryConfiguration : EntityTypeConfigurationBase<Inventory, Guid>
    {
        public override void Configure(EntityTypeBuilder<Inventory> builder)
        {
            //builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}
