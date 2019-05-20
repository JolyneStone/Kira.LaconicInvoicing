using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kira.LaconicInvoicing
{
    [Serializable]
    public class SortDescription
    {
        public string Field { get; set; }
        public string Order { get; set; }

        public SortDescription()
        {
        }

        public SortDescription(string field, string order)
        {
            Field = field;
            Order = order;
        }
    }
}
