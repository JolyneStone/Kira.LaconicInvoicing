using System;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Security.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Core.EntityInfos;
using OSharp.Entity;

namespace Kira.LaconicInvoicing.EntityConfiguration.Security
{
    public class EntityUserConfiguration : EntityTypeConfigurationBase<EntityUser, Guid>
    {
        /// <summary>
        /// ��д��ʵ��ʵ�����͸������Ե����ݿ�����
        /// </summary>
        /// <param name="builder">ʵ�����ʹ�����</param>
        public override void Configure(EntityTypeBuilder<EntityUser> builder)
        {
            builder.HasIndex(m => new { m.EntityId, m.UserId }).HasName("EntityUserIndex");

            builder.HasOne<EntityInfo>(eu => eu.EntityInfo).WithMany().HasForeignKey(m => m.EntityId);
            builder.HasOne<User>(eu => eu.User).WithMany().HasForeignKey(m => m.UserId);
        }
    }
}