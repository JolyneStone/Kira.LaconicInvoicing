using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Kira.LaconicInvoicing.Sale.Entities
{
    public class CustomerProduct: EntityBase<Guid>
    {
        [Required]
        public Guid CustomerId { get; set; }
        [Required]
        public Guid ProductId { get; set; }
    }
}
