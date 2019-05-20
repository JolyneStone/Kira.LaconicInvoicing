using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;

namespace Kira.LaconicInvoicing.EntityConfiguration.Purchase
{
    public class VendorConfiguration : EntityTypeConfigurationBase<Vendor, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<Vendor> builder)
        {
            builder.HasIndex(v => v.Number).IsUnique();
        }
    }
}
