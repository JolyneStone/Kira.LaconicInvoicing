using Kira.LaconicInvoicing.File.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.EntityConfiguration.File
{
    public class FileTemplateConfiguration : EntityTypeConfigurationBase<FileTemplate, Guid>
    {
        public override void Configure(EntityTypeBuilder<FileTemplate> builder)
        {
        }
    }
}
