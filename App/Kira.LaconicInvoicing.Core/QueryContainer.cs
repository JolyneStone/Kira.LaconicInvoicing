using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Kira.LaconicInvoicing
{
    [Serializable]
    public sealed class QueryContainer
    {
        public List<QueryConditionDescription> Filters { get; set; }

        public List<SortDescription> Sorts { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        /// <summary>
        /// 初始化 <see cref="QueryCondition{T}"/> 类的新实例。
        /// </summary>
        public QueryContainer()
        {
            this.Filters = new List<QueryConditionDescription>();
            this.Sorts = new List<SortDescription>();
        }

        public QueryCondition<T> ToQueryCondition<T>()
        {
            var condition = new QueryCondition<T>
            {
                Filters = Filters.Select(q => q.ToConditionItem()).ToList(),
                Sorts = Sorts,
                PageSize = PageSize,
                PageIndex = PageIndex
            };
            return condition;
        }
    }
}
