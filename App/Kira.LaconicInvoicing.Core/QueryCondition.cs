using Kira.LaconicInvoicing;
using OSharp.Extensions;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.Serialization;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 查询条件。
    /// </summary>
    /// <typeparam name="T">要查询的元素的类型。</typeparam>
    [Serializable]
    public class QueryCondition<T>
    {
        private Type _sourceType = null;

        private List<QueryConditionItem> _filters;

        private List<SortDescription> _sorts;

        private int _pageSize;

        private int _pageIndex;

        public List<QueryConditionItem> Filters { get => _filters; set => _filters = value; }

        public Type SourceType
        {
            get
            {
                if (_sourceType == null)
                {
                    _sourceType = typeof(T);
                }
                return _sourceType;
            }
        }

        public List<SortDescription> Sorts { get => _sorts; set => _sorts = value; }

        public int PageSize
        {
            get
            {
                if (_pageSize < 0)
                {
                    _pageSize = 0;
                }

                return _pageSize;
            }
            set
            {
                _pageSize = value;
            }
        }

        public int PageIndex
        {
            get
            {
                if (_pageIndex < 0)
                {
                    _pageIndex = 0;
                }

                return _pageIndex;
            }
            set
            {
                _pageIndex = value;
            }
        }

        /// <summary>
        /// 初始化 <see cref="QueryCondition{T}"/> 类的新实例。
        /// </summary>
        public QueryCondition()
        {
            this._filters = new List<QueryConditionItem>();
            this._sorts = new List<SortDescription>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        public QueryCondition<T> AddSort(string field, string order)
        {
            return AddSort(new SortDescription(field, order));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sort"></param>
        /// <returns></returns>
        public QueryCondition<T> AddSort(SortDescription sort)
        {
            this._sorts.Add(sort);
            return this;
        }

        /// <summary>
        /// 清空所有条件。
        /// </summary>
        public void Clear()
        {
            _filters.Clear();
            _sorts.Clear();
        }

        public void ClearFilter()
        {
            this._filters.Clear();
        }

        public void ClearSort()
        {
            this._sorts.Clear();
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, string value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, bool value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, int value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, uint value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, long value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, ulong value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, short value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, ushort value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, byte value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, sbyte value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, float value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, double value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, decimal value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, Guid value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, DateTime value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, (object)value, logicOperator);
        }

        public QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, Enum value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter(field, conditionOperator, value.ToString(), logicOperator);
        }

        private QueryCondition<T> AddFilter(string field, ConditionOperator conditionOperator = ConditionOperator.Equal, object value = default, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            if (null == field)
                throw new ArgumentNullException("field");

            if (String.IsNullOrWhiteSpace(field))
                throw new ArgumentException("字段名称不能为空字符串或仅由空白字符组成的字符串。", "field");

            if (null == SourceType.GetProperty(field))
                throw new ArgumentException(String.Format("类型“{0}”不存在名为“{1}”的属性。", SourceType.Name, field), "field");

            _filters.Add(new QueryConditionItem(logicOperator, field, conditionOperator, value));
            return this;
        }

        public QueryCondition<T> AddFilter<TValue>(Expression<Func<T, TValue>> fieldSelector, ConditionOperator conditionOperator = ConditionOperator.Equal, TValue value = default(TValue), LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            if (null == fieldSelector)
                throw new ArgumentNullException("fieldSelector");

            var expr = (fieldSelector.Body is UnaryExpression) ? ((fieldSelector.Body as UnaryExpression).Operand as MemberExpression) : fieldSelector.Body as MemberExpression;
            if (null == expr)
                throw new ArgumentException(String.Format("“{0}”不是有效的属性访问表达式。", fieldSelector), "fieldSelector");

            if (expr.Type != typeof(string) && conditionOperator.In(ConditionOperator.Contains, ConditionOperator.EndsWith, ConditionOperator.StartsWith))
                throw new ArgumentException("“{0}”只能用于比较字符串类型。".FormatWith(conditionOperator), "conditionOperator");

            if (expr.Type.IsEnum)
                return AddFilter(expr.Member.Name, conditionOperator, value.ToString(), logicOperator);

            return AddFilter(expr.Member.Name, conditionOperator, value, logicOperator);
        }

        public QueryCondition<T> AddStringCondition(Expression<Func<T, string>> fieldSelector, ConditionOperator conditionOperator = ConditionOperator.Equal, string value = null, LogicOperator logicOperator = LogicOperator.AndAlso)
        {
            return AddFilter<string>(fieldSelector, conditionOperator, value, logicOperator);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return GetConditionString();
        }



        #region 扩展 使此类可实现（SZDY = 111 && (MC = "" || Other = "")） 类似查询
        /// <summary>
        /// 子查询之间的逻辑关系，，默认 AndAlso
        /// </summary>
        [DataMember]
        public LogicOperator LogicOpForChildren { get; set; }
        /// <summary>
        /// 本实例与子查询条件的逻辑关系，默认 AndAlso
        /// </summary>
        [DataMember]
        public LogicOperator LogicOp { get; set; }

        [DataMember]
        private List<QueryCondition<T>> _children = new List<QueryCondition<T>>();
        /// <summary>
        /// 添加子查询条件
        /// </summary>
        /// <param name="child"></param>
        public void Add(QueryCondition<T> child)
        {
            _children.Add(child);
        }

        #region 为了toString
        // 获取当前实例的查询条件不包含子查询 条件
        private string GetCurrentConditionString()
        {
            if (null == _filters || 1 > _filters.Count)
                return String.Empty;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (var i = 0; i < _filters.Count; i++)
            {
                QueryConditionItem item = _filters[i];
                if (i != 0)
                    sb.AppendFormat(" {0} ", item.LogicOp == LogicOperator.AndAlso ? "&&" : "||");

                sb.AppendFormat("{0} {1} {2}", item.Field, item.ConditionOp, item.Value);
            }

            string str = sb.ToString();


            return !String.IsNullOrWhiteSpace(str) ? "(" + str + ")" : "";
        }
        // 获取当前实例的查询条件包含子查询 条件
        private string GetConditionString()
        {
            string currentConStr = GetCurrentConditionString();
            string conditionStr = currentConStr;
            string childrenConStr = "";

            string logicForChildren = LogicOpForChildren == LogicOperator.AndAlso ? "&&" : "||";
            string logic = LogicOp == LogicOperator.AndAlso ? "&&" : "||";

            if (_children != null)
            {
                foreach (var item in _children)
                {
                    string itemConStr = item.GetConditionString();
                    if (!String.IsNullOrWhiteSpace(itemConStr))
                    {
                        childrenConStr += (!string.IsNullOrWhiteSpace(childrenConStr) ? logicForChildren : "") + itemConStr;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(childrenConStr))
            {
                conditionStr += logic + childrenConStr;
            }

            return conditionStr;
        }

        #endregion

        #region 生成where 直接可用的查询条件
        // 获取当前实例的查询条件不包含子查询 条件
        private string GetCurrentFilter()
        {
            Type sourceType = typeof(T);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();

            for (var i = 0; i < _filters.Count; i++)
            {
                QueryConditionItem item = _filters[i];
                if (item == null || string.IsNullOrWhiteSpace(item.Field) || item.Value == null) continue;

                if (i != 0)
                    sb.AppendFormat(" {0} ", item.LogicOp == LogicOperator.AndAlso ? "&&" : "||");

                switch (item.ConditionOp)
                {
                    case ConditionOperator.Equal:
                    case ConditionOperator.GreaterThan:
                    case ConditionOperator.GreaterThanOrEqual:
                    case ConditionOperator.LessThan:
                    case ConditionOperator.NotEqual:
                    case ConditionOperator.LessThanOrEqual:
                        sb.AppendFormat("{0} {1} @p", item.Field, BuildConditionOperator(item.ConditionOp));
                        break;
                    case ConditionOperator.Contains:
                    case ConditionOperator.EndsWith:
                    case ConditionOperator.StartsWith:
                        sb.AppendFormat("{0}.{1}(@p)", item.Field, item.ConditionOp);
                        break;
                }
                var propInfo = sourceType.GetProperty(item.Field);
            }
            return !string.IsNullOrWhiteSpace(sb.ToString()) ? "(" + sb.ToString() + ")" : "";
        }

        private string GetFilterTemp()
        {
            string currentConStr = GetCurrentFilter();
            string conditionStr = currentConStr;
            string childrenConStr = "";

            string logicForChildren = LogicOpForChildren == LogicOperator.AndAlso ? "&&" : "||";
            string logic = LogicOp == LogicOperator.AndAlso ? "&&" : "||";

            if (_children != null)
            {
                foreach (var item in _children)
                {
                    string itemConStr = item.GetFilterTemp();
                    if (!string.IsNullOrWhiteSpace(itemConStr))
                    {
                        childrenConStr += (!string.IsNullOrWhiteSpace(childrenConStr) ? logicForChildren : "") + itemConStr;
                    }
                }
            }

            if (!string.IsNullOrWhiteSpace(childrenConStr))
            {

                conditionStr += (!string.IsNullOrWhiteSpace(conditionStr) ? logic : "") + childrenConStr;

            }

            return conditionStr;
        }

        /// <summary>
        ///   获取当前实例的查询条件包含子查询 条件
        /// </summary>
        public string GetFilter()
        {
            string filterStringTemp = GetFilterTemp();
            if (!string.IsNullOrWhiteSpace(filterStringTemp))
            {
                int index = 0;
                filterStringTemp = filterStringTemp.Replace("@p", m =>
                {
                    return "@" + (index++);
                });
            }
            return filterStringTemp;
        }


        public object[] GetValues()
        {
            List<object> values = new List<object>();
            if (Filters != null)
            {
                foreach (var c in Filters)
                {
                    if (c != null && c.Value != null && c.Field.IsNotNullOrEmpty())
                    {
                        var properinfo = SourceType.GetProperty(c.Field);
                        if (properinfo.PropertyType.IsEnum)
                        {
                            values.Add(Enum.Parse(properinfo.PropertyType, c.Value.ToString()));
                        }
                        else
                        {
                            values.Add(c.Value);
                        }
                    }
                }
            }
            if (_children != null)
            {
                _children.ForEach(c => { values.AddRange(c.GetValues()); });
            }
            return values.ToArray();
        }

        /// <summary>
        /// 解析条件比较符号
        /// </summary>
        /// <param name="op"></param>
        /// <returns></returns>
        public static string BuildConditionOperator(ConditionOperator op)
        {
            switch (op)
            {
                case ConditionOperator.GreaterThan:
                    return ">";
                case ConditionOperator.GreaterThanOrEqual:
                    return ">=";
                case ConditionOperator.LessThan:
                    return "<";
                case ConditionOperator.LessThanOrEqual:
                    return "<=";
                case ConditionOperator.NotEqual:
                    return "!=";
                default:
                case ConditionOperator.Equal:
                    return "==";
            }
        }



        /// <summary>
        /// 为了与前端配合
        /// </summary>
        private static readonly IDictionary<string, string> dicOperators = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase) {
           {"eq","Equal"},
           { "neq","NotEqual"},
           { "lt","LessThan"},
           { "lte","LessThanOrEqual"},
           { "gt","GreaterThan"},
           { "gte","GreaterThanOrEqual"},
           { "startswith","StartsWith"},
           { "endswith","EndsWith"},
           { "contains","Contains"}
        };

        /// <summary>
        /// 解析前端逻辑比较符 返回值为int 为了兼容 ConditionOperator  ConditionOperator  ConditionOperator
        /// </summary>
        /// <param name="frontEndOptr"></param>
        /// <returns></returns>
        public static int ParseOperatorFromWeb(string frontEndOptr)
        {
            string optr = dicOperators[frontEndOptr];
            if (optr == null)
            {
                throw new Exception("不存在的逻辑比较符：" + frontEndOptr);
            }

            var eOptr = (ConditionOperator)Enum.Parse(typeof(ConditionOperator), optr, true);

            return (int)eOptr;
        }

        #endregion

        #endregion
    }

    /// <summary>
    /// 枚举逻辑操作符。
    /// </summary>
    [Serializable]
    public enum LogicOperator
    {
        /// <summary>
        /// 与。
        /// </summary>
        AndAlso = 0,

        /// <summary>
        /// 或。
        /// </summary>
        OrElse = 1
    }

    /// <summary>
    /// 枚举条件比较操作符。
    /// </summary>
    [Serializable]
    public enum ConditionOperator
    {
        /// <summary>
        /// 等于。
        /// </summary>
        [EnumDisplayName("等于")]
        Equal = 0,

        /// <summary>
        /// 小于。
        /// </summary>
        [EnumDisplayName("小于")]
        LessThan = 1,

        /// <summary>
        /// 小于等于。
        /// </summary>
        [EnumDisplayName("小于等于")]
        LessThanOrEqual = 2,

        /// <summary>
        /// 大于。
        /// </summary>
        [EnumDisplayName("大于")]
        GreaterThan = 3,

        /// <summary>
        /// 大于等于。
        /// </summary>
        [EnumDisplayName("大于等于")]
        GreaterThanOrEqual = 4,

        /// <summary>
        /// 开始于。
        /// </summary>
        [EnumDisplayName("开始于")]
        StartsWith = 5,

        /// <summary>
        /// 结束于。
        /// </summary>
        [EnumDisplayName("结束于")]
        EndsWith = 6,

        /// <summary>
        /// 包含。
        /// </summary>
        [EnumDisplayName("包含")]
        Contains = 7,
        /// <summary>
        /// 不等于。
        /// </summary>
        [EnumDisplayName("不等于")]
        NotEqual = 8
    }

    ///// <summary>
    ///// 枚举条件比较操作符。
    ///// </summary>
    //[Serializable]
    //public enum ConditionOperator
    //{
    //    /// <summary>
    //    /// 等于。
    //    /// </summary>
    //    [EnumDisplayName("等于")]
    //    Equal = 0,

    //    /// <summary>
    //    /// 小于。
    //    /// </summary>
    //    [EnumDisplayName("小于")]
    //    LessThan = 1,

    //    /// <summary>
    //    /// 小于等于。
    //    /// </summary>
    //    [EnumDisplayName("小于等于")]
    //    LessThanOrEqual = 2,

    //    /// <summary>
    //    /// 大于。
    //    /// </summary>
    //    [EnumDisplayName("大于")]
    //    GreaterThan = 3,

    //    /// <summary>
    //    /// 大于等于。
    //    /// </summary>
    //    [EnumDisplayName("大于等于")]
    //    GreaterThanOrEqual = 4,
    //    /// <summary>
    //    /// 不等于。
    //    /// </summary>
    //    [EnumDisplayName("不等于")]
    //    NotEqual = 8
    //}

    ///// <summary>
    ///// 枚举字符串条件比较操作符。
    ///// </summary>
    //[Serializable]
    //public enum ConditionOperator
    //{
    //    /// <summary>
    //    /// 等于。
    //    /// </summary>
    //    [EnumDisplayName("等于")]
    //    Equal = 0,

    //    /// <summary>
    //    /// 开始于。
    //    /// </summary>
    //    [EnumDisplayName("开始于")]
    //    StartsWith = 5,

    //    /// <summary>
    //    /// 结束于。
    //    /// </summary>
    //    [EnumDisplayName("结束于")]
    //    EndsWith = 6,

    //    /// <summary>
    //    /// 包含。
    //    /// </summary>
    //    [EnumDisplayName("包含")]
    //    Contains = 7,
    //    /// <summary>
    //    /// 不等于。
    //    /// </summary>
    //    [EnumDisplayName("不等于")]
    //    NotEqual = 8
    //}

    /// <summary>
    /// 查询条件内容项。
    /// </summary>
    [Serializable]
    public class QueryConditionItem
    {
        /// <summary>
        /// 获取与前一个条件的连接的逻辑操作符。
        /// </summary>
        public LogicOperator LogicOp { get; set; }

        /// <summary>
        /// 获取条件中用于比较的字段名。
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 获取条件中用于比较的操作符。
        /// </summary>
        public ConditionOperator ConditionOp { get; set; }

        /// <summary>
        /// 获取条件用于比较的值。
        /// </summary>
        public object Value { get; set; }

        /// <summary>
        /// 初始化 <see cref="QueryConditionItem"/> 类的新实例。
        /// </summary>
        /// <param name="field">组成条件的字段名。</param>
        /// <param name="conditionOperator">组成条件的比较操作符。</param>
        /// <param name="value">条件中用于比较的值。</param>
        public QueryConditionItem(string field, ConditionOperator conditionOperator, object value)
        {
            this.Field = field;
            this.Value = value;
            this.ConditionOp = conditionOperator;
        }

        /// <summary>
        /// 初始化 <see cref="QueryConditionItem"/> 类的新实例。
        /// </summary>
        /// <param name="logicOperator">连接前一个条件的逻辑操作符。</param>
        /// <param name="field">组成条件的字段名。</param>
        /// <param name="conditionOperator">组成条件的比较操作符。</param>
        /// <param name="value">条件中用于比较的值。</param>
        public QueryConditionItem(LogicOperator logicOperator, string field, ConditionOperator conditionOperator, object value)
            : this(field, conditionOperator, value)
        {
            this.LogicOp = logicOperator;
        }
    }
}