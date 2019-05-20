using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using WarehouseEntity = Kira.LaconicInvoicing.Warehouse.Entities.Warehouse;

namespace Kira.LaconicInvoicing.EntityConfiguration.Warehouse
{
    public class WarehouseConfiguration : EntityTypeConfigurationBase<WarehouseEntity, Guid>
    {
        public override void Configure(EntityTypeBuilder<WarehouseEntity> builder)
        {
            builder.HasIndex(p => p.Number).IsUnique();
        }
    }
}
