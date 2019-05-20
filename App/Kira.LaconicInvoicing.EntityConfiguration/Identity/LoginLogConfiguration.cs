using System;
using Kira.LaconicInvoicing.Identity.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;


namespace Kira.LaconicInvoicing.EntityConfiguration.Identity
{
    public class LoginLogConfiguration : EntityTypeConfigurationBase<LoginLog, Guid>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<LoginLog> builder)
        {
            builder.HasOne<User>(m => m.User).WithMany().HasForeignKey(m => m.UserId).OnDelete(DeleteBehavior.Cascade);
        }
    }
}