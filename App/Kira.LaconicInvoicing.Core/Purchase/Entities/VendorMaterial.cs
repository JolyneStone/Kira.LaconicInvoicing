using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Purchase.Entities
{
    public class VendorMaterial: EntityBase<Guid>
    {
        [Required]
        public Guid VendorId { get; set; }
        [Required]
        public Guid MaterialId { get; set; }

        public virtual List<Vendor> Vendors { get; set; }

        //public virtual List<Material> Materials { get; set; }
    }
}
