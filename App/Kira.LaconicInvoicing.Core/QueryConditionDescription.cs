using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    [Serializable]
    public class QueryConditionDescription
    {
        /// <summary>
        /// 获取与前一个条件的连接的逻辑操作符。
        /// </summary>
        public string LogicOp { get; set; }

        /// <summary>
        /// 获取条件中用于比较的字段名。
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 获取条件中用于比较的操作符。
        /// </summary>
        public string ConditionOp { get; set; }

        /// <summary>
        /// 获取条件用于比较的值。
        /// </summary>
        public object Value { get; set; }

        public QueryConditionItem ToConditionItem()
        {
            return new QueryConditionItem((LogicOperator)Enum.Parse(typeof(LogicOperator), LogicOp), Field, (ConditionOperator)Enum.Parse(typeof(ConditionOperator), ConditionOp), Value);
        }
    }
}
