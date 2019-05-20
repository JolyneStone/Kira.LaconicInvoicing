using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 提供相关描述枚举常数值的信息。
    /// </summary>
    [Serializable]
    public class EnumField
    {
        /// <summary>
        /// 获取枚举值（根据其基础类型）。
        /// </summary>
        public object Value
        {
            get
            {
                return this._value;
            }
        }

        /// <summary>
        /// 获取枚举常数值的显示名称。
        /// </summary>
        public string DisplayName
        {
            get
            {
                return this._displayName;
            }
        }

        /// <summary>
        /// 获取一个 <see cref="T:System.Boolean" /> 值，该值表示是否是枚举值默认的显示名称。
        /// </summary>
        public bool IsDefaultDisplayName { get; private set; }

        /// <summary>
        /// 获取枚举常数值的描述信息。
        /// </summary>
        public string Description
        {
            get
            {
                return this._description;
            }
        }

        private EnumField(object value, string displayName, string description)
        {
            this._value = value;
            this._displayName = displayName;
            this._description = description;
        }

        /// <summary>
        /// 将此实例的值转换为其等效的字符串表示。
        /// </summary>
        /// <returns>此实例的值的字符串表示。</returns>
        public override string ToString()
        {
            return this._value.ToString();
        }

        /// <summary>
        /// 返回此实例的哈希代码。
        /// </summary>
        /// <returns>32 位有符号整数哈希代码。</returns>
        public override int GetHashCode()
        {
            return this._value.GetHashCode();
        }

        /// <summary>
        /// 获取枚举常数值的相关信息。
        /// </summary>
        /// <param name="enumValue">枚举常数值。</param>
        /// <returns>一个 <see cref="T:YuLinTu.EnumField" /> 的实例，它包含枚举常数值的一些信息。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="enumValue" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="enumValue" /> 既不是枚举类型，也不是枚举基础类型，如 Int32。</exception>
        public static EnumField GetField(object enumValue)
        {
            if (enumValue == null)
            {
                throw new ArgumentNullException("enumValue");
            }
            Type type = enumValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("传入的值必须是枚举的枚举基或基础类型，如 Int32。");
            }
            EnumField[] fields = EnumField.GetFields(type, false);
            foreach (EnumField enumField in fields)
            {
                if (enumField.Value.Equals(enumValue))
                {
                    return enumField;
                }
            }
            return new EnumField(enumValue, enumValue.ToString(), string.Empty);
        }

        /// <summary>
        /// 获取指定枚举类型的所有枚举字段信息。
        /// </summary>
        /// <param name="enumType">要获取其枚举字段信息的枚举。</param>
        /// <param name="ignoreDefaultDisplayName">指定是否忽略默认显示名称的枚举字段。</param>
        /// <returns>描述 <paramref name="enumType" /> 中所有枚举字段信息的 <see cref="T:YuLinTu.EnumField" /> 数组。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="enumType" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException"><paramref name="enumType" /> 参数不是 <see cref="T:System.Enum" />。</exception>
        public static EnumField[] GetFields(Type enumType, bool ignoreDefaultDisplayName = false)
        {
            if (null == enumType)
            {
                throw new ArgumentNullException("enumType");
            }
            if (!enumType.IsEnum)
            {
                throw new ArgumentException("提供的类型必须是 Enum。");
            }
            EnumField[] fieldsInternal = EnumField.GetFieldsInternal(enumType);
            if (ignoreDefaultDisplayName)
            {
                return (from f in fieldsInternal
                        where !f.IsDefaultDisplayName
                        select f).ToArray<EnumField>();
            }
            return fieldsInternal;
        }

        private static EnumField[] GetFieldsInternal(Type enumType)
        {
            EnumField[] array = (EnumField[])EnumField.cachedHash[enumType];
            if (array == null)
            {
                if (EnumField.cachedHash.Count > 200)
                {
                    EnumField.cachedHash.Clear();
                }
                FieldInfo[] fields = enumType.GetFields(BindingFlags.Static | BindingFlags.Public);
                array = new EnumField[fields.Length];
                for (int i = 0; i < fields.Length; i++)
                {
                    FieldInfo enumFieldInfo = fields[i];
                    array[i] = EnumField.GetEnumField(enumFieldInfo);
                }
                EnumField.cachedHash[enumType] = array;
            }
            return array;
        }

        private static EnumField GetEnumField(FieldInfo enumFieldInfo)
        {
            if (null == enumFieldInfo)
            {
                return null;
            }
            string name = enumFieldInfo.Name;
            string empty = string.Empty;
            object value = enumFieldInfo.GetValue(null);
            EnumField enumField = new EnumField(value, name, empty);
            object[] customAttributes = enumFieldInfo.GetCustomAttributes(typeof(EnumDisplayNameAttribute), false);
            enumField.IsDefaultDisplayName = true;
            if (customAttributes.Length > 0)
            {
                enumField._displayName = (customAttributes[0] as EnumDisplayNameAttribute).DisplayName;
                enumField.IsDefaultDisplayName = false;
            }
            customAttributes = enumFieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
            if (customAttributes.Length > 0)
            {
                enumField._description = (customAttributes[0] as DescriptionAttribute).Description;
            }
            return enumField;
        }

        private object _value;

        private string _displayName;

        private string _description;

        private static Hashtable cachedHash = Hashtable.Synchronized(new Hashtable());
    }
}
