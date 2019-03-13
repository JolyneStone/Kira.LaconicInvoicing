using System;
using Microsoft.EntityFrameworkCore;
using OSharp.Entity;

namespace Kira.LaconicInvoicing.Domain
{
    public class BusinessDbContext : DefaultDbContext
    {
        public BusinessDbContext(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder) : base(options, typeFinder)
        {
        }
    }
}
