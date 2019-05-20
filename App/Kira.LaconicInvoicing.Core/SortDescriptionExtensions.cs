using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kira.LaconicInvoicing
{
    public static class SortDescriptionExtensions
    {
        /// <summary>
        /// 转化成排序字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ConvertToSortString(this SortDescription @this)
        {
            if (@this != null)
            {
                return string.Format("{0} {1}", @this.Field, @this.Order);
            }

            return null;
        }

        /// <summary>
        /// 转化成排序字符串
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static string ConvertToSortString(this IList<SortDescription> @this)
        {
            if (@this != null && @this.Count > 0)
            {
                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < @this.Count; i++)
                {
                    if (!string.IsNullOrWhiteSpace(@this[i].Field) && !string.IsNullOrWhiteSpace(@this[i].Order))
                    {
                        var order = @this[i].Order.ToUpperInvariant().Replace("ASCEND", "ASC").Replace("DESCEND", "DESC");
                        if (sb.Length == 0)
                        {
                            sb.AppendFormat("{0} {1}", @this[i].Field, order);
                        }
                        else
                        {
                            sb.AppendFormat(", {0} {1}", @this[i].Field, order);
                        }
                    }
                }

                return sb.ToString();
            }

            return null;
        }
    }
}
