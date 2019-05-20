using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 表示查询过滤条件信息。
    /// </summary>
    [Serializable]
    public class QueryFilter
    {
        /// <summary>
        /// 获取所有的查询过滤项。
        /// </summary>
        public QueryFilterItem[] Items { get; private set; }

        /// <summary>
        /// 获取所有查询过滤项之间的逻辑操作符。
        /// </summary>
        public LogicOperator Logic { get; private set; }

        /// <summary>
        /// 初始化 <see cref="QueryFilter"/> 类的新实例。
        /// </summary>
        /// <param name="filterItems">查询过滤项。</param>
        public QueryFilter(IEnumerable<QueryFilterItem> filterItems) : this(filterItems, LogicOperator.AndAlso) { }

        /// <summary>
        /// 初始化 <see cref="QueryFilter"/> 类的新实例。
        /// </summary>
        /// <param name="filterItems">查询过滤项。</param>
        /// <param name="logic"><see cref="LogicOperator"/> 值之一，表示所有查询过滤项之间的逻辑操作符。</param>
        public QueryFilter(IEnumerable<QueryFilterItem> filterItems, LogicOperator logic)
        {
            if (null == filterItems)
                throw new ArgumentNullException("filterItems");

            this.Logic = logic;
            Items = filterItems.Where(f => null != f).ToArray();
        }
    }

    /// <summary>
    /// 表示查询过滤条件中的过滤项及值信息。
    /// </summary>
    public class QueryFilterItem
    {
        /// <summary>
        /// 获取或设置要过滤字段的名称。
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 获取或设置要过滤字段与值的比较操作符。
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// 获取或设置要过滤的字段的过滤值。
        /// </summary>
        public string Value { get; set; }
    }
}
