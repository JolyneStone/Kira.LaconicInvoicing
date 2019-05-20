using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
{
    public class SimpleWarehouseDto: IOutputDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
