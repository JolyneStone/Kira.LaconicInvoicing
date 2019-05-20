// -----------------------------------------------------------------------
using Kira.LaconicInvoicing.Identity.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;

namespace Kira.LaconicInvoicing.EntityConfiguration.Identity
{
    public class UserClaimConfiguration : EntityTypeConfigurationBase<UserClaim, int>
    {
        /// <summary>
        /// 重写以实现实体类型各个属性的数据库配置
        /// </summary>
        /// <param name="builder">实体类型创建器</param>
        public override void Configure(EntityTypeBuilder<UserClaim> builder)
        {
            builder.HasOne(uc => uc.User).WithMany(u => u.UserClaims).HasForeignKey(uc => uc.UserId).IsRequired();
        }
    }
}