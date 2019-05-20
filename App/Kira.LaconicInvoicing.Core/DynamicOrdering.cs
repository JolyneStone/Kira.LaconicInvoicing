using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace Kira.LaconicInvoicing
{
    internal class DynamicOrdering
    {
        public Expression Selector;

        public bool Ascending;
    }
}
