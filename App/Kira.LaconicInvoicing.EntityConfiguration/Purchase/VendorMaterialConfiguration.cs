using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;

namespace Kira.LaconicInvoicing.EntityConfiguration.Purchase
{
    public class VendorMaterialConfiguration : EntityTypeConfigurationBase<VendorMaterial, Guid>
    {
        public override void Configure(EntityTypeBuilder<VendorMaterial> builder)
        {
            //builder.HasOne(m => m.Vendor)
            //    .WithMany(v => v.VendorMaterials)
            //    .HasForeignKey(m => m.VendorId)
            //    .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);

            //builder.HasOne(m => m.Material)
            //    .WithMany(v => v.VendorMaterials)
            //    .HasForeignKey(m => m.MaterialId)
            //    .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.SetNull);
        }
    }
}
