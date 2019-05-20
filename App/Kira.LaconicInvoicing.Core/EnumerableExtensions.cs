using Microsoft.EntityFrameworkCore;
using OSharp.Filter;
using System.Collections.Generic;
using System.Linq;

namespace Kira.LaconicInvoicing
{
    public static class EnumerableExtensions
    {
        public static PageData<T> ToPageData<T>(this IEnumerable<T> @this, int total)
        {
            if (@this == null)
                return new PageData<T>();

            return new PageData<T>(@this, total);
        }
    }
}
