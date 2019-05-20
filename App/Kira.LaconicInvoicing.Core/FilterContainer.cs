using System;
using System.Text;
using System.Linq;
using System.Reflection;
using System.ComponentModel;
using System.Collections.Generic;

namespace Kira.LaconicInvoicing
{
    [Serializable]
    public class FilterContainer
    {
        public List<FilterDescription> Filters { get; set; }
        public string Logic { get; set; }

        public override string ToString()
        {
            if (null == Filters || 1 > Filters.Count)
                return null;

            string logicOp = String.Equals(Logic, "and", StringComparison.CurrentCultureIgnoreCase) ? "and" : "or";
            return String.Join(logicOp, Filters.Select(fi => fi.ToString()).ToArray());
        }

        public QueryFilter BuildQueryFilter(Type queryEntityType = null)
        {
            if (null == Filters || 1 > Filters.Count)
                return null;

            LogicOperator logicOperator = String.Equals(Logic, "and", StringComparison.OrdinalIgnoreCase) ? LogicOperator.AndAlso : LogicOperator.OrElse;
            var filterItems = Filters.Where(f => null != f).Select(f => f.ToFilterItem()).ToArray();
            if (null == filterItems || 1 > filterItems.Length)
                return null;

            if (null != queryEntityType)
            {
                var props = queryEntityType.GetProperties();
                foreach(var f in filterItems)
                {
                    f.Field = GetFieldName(f.Field, props);
                }
            }

            return new QueryFilter(filterItems, logicOperator);
        }

        #region 常用方法

        /// <summary>
        /// 获取关键字查询值  
        /// </summary>
        /// <returns></returns>
        public object GetKeyFilterValue()
        {
            return GetValueByField("key");
        }

        /// <summary>
        /// 获取值
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public object GetValueByField(string field)
        {
            if (Filters == null) return null;
            FilterDescription filter = Filters.FirstOrDefault(c => c.Field == field);
            return filter == null ? null : filter.Value;
        }

        #endregion


        private string GetFieldName(string field, PropertyInfo[] props)
        {
            if (null == props || 1 > props.Length)
                return field;

            var prop = props.FirstOrDefault(p => String.Equals(p.Name, field, StringComparison.OrdinalIgnoreCase));
            if (null == prop)
                return field;

            return prop.Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        public T GetFilterItemValue<T>(string field)
        {
            if (null == field)
                throw new ArgumentNullException("field");

            if (0 == field.Trim().Length)
                throw new ArgumentException("字段的名称不能为空或仅由空白字符组成的字符串。");

            if (null == Filters || 1 > Filters.Count)
                return default(T);

            var filterDes = Filters.FirstOrDefault(f => String.Equals(field, f.Field, StringComparison.OrdinalIgnoreCase));
            if (null == filterDes)
                return default(T);

            return (T)ConvertSimpleType(filterDes.Value, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="queryEntityType"></param>
        /// <returns></returns>
        public CodeDynamicQuery BuildDynamicQuery(Type queryEntityType)
        {
            if (null == queryEntityType)
                throw new ArgumentNullException("queryEntityType");

            var props = queryEntityType.GetProperties();
            var filterItems = GetFilterItems(props);
            if (null == filterItems || 1 > filterItems.Length)
                return null;

            string logicOp = String.Equals(Logic, "and", StringComparison.OrdinalIgnoreCase) ? "and" : "or";
            string code = String.Join(logicOp, filterItems.Select((f, i) => f.ToCodeString(i)).ToArray());
            return new CodeDynamicQuery(code, filterItems.Select(f => f.Value).ToArray());
        }

        private TypeFilterItem[] GetFilterItems(PropertyInfo[] props)
        {
            if (null == Filters || 1 > Filters.Count)
                return null;

            var filterItems = new List<TypeFilterItem>(Filters.Count);
            foreach (var filterDes in Filters)
            {
                var filterItem = GetFilterItem(filterDes, props);
                if (null != filterItem)
                    filterItems.Add(filterItem);
            }

            return filterItems.ToArray();
        }

        private TypeFilterItem GetFilterItem(FilterDescription filterDes, PropertyInfo[] props)
        {
            if (null == filterDes || null == props || 1 > props.Length)
                return null;

            var prop = props.FirstOrDefault(p => String.Equals(p.Name, filterDes.Field, StringComparison.OrdinalIgnoreCase));
            if (null == prop)
                return null;

            string fieldName = prop.Name;
            var value = ConvertSimpleType(filterDes.Value, prop.PropertyType);
            if (null == value)
                return null;

            return new TypeFilterItem { FieldName = fieldName, Value = value, Operator = filterDes.CodeOperator, IsStringMethodOperator = filterDes.IsStringMethodOperator };
        }

        private static object ConvertSimpleType(object value, Type destinationType)
        {
            if (value == null || destinationType.IsInstanceOfType(value))
                return value;

            string text = value as string;
            if (text != null && text.Trim().Length == 0)
                return null;

            if (destinationType.IsEnum)
            {
                if (Enum.IsDefined(destinationType, value))
                    return Enum.Parse(destinationType, value.ToString(), true);
            }

            var converter = TypeDescriptor.GetConverter(destinationType);
            bool flag = converter.CanConvertFrom(value.GetType());
            if (!flag)
                converter = TypeDescriptor.GetConverter(value.GetType());

            if (!flag && !converter.CanConvertTo(destinationType))
                return null;

            object result;
            try
            {
                object obj = flag ? converter.ConvertFrom(value) : converter.ConvertTo(value, destinationType);
                result = obj;
            }
            catch
            {
                return null;
            }
            return result;
        }

        private class TypeFilterItem
        {
            public string FieldName { get; set; }
            public object Value { get; set; }
            public string Operator { get; set; }
            public bool IsStringMethodOperator { get; set; }
            public override string ToString()
            {
                return IsStringMethodOperator
                    ? String.Format("{0}.{1}(\"{2}\")", FieldName, Operator, Value)
                    : String.Format("{0} {1} {2}", FieldName, Operator, Value);
            }

            public string ToCodeString(int argumentIndex)
            {
                return IsStringMethodOperator
                    ? String.Format("{0}.{1}(@{2})", FieldName, Operator, argumentIndex)
                    : String.Format("{0} {1} @{2}", FieldName, Operator, argumentIndex);
            }
        }
    }
}