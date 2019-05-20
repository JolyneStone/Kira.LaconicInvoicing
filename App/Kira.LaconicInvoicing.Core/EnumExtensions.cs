using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 提供一组 <see cref="T:System.Enum" /> 对象的常用 static（在 Visual Basic 中为 Shared）方法。
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        /// 获取指定枚举常数值的显示名称。
        /// </summary>
        /// <param name="source">枚举常数值。</param>
        /// <returns>一个字符串，表示指定枚举常数值的显示名称。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        public static string GetDisplayName(this Enum source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return EnumField.GetField(source).DisplayName;
        }

        /// <summary>
        /// 获取指定枚举常数值的描述信息。
        /// </summary>
        /// <param name="source">枚举常数值。</param>
        /// <returns>一个字符串，表示指定枚举常数值的描述信息。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        public static string GetDescription(this Enum source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return EnumField.GetField(source).Description;
        }

        /// <summary>
        /// 获取指定枚举类型的所有枚举字段信息。
        /// </summary>
        /// <param name="source">要获取其枚举字段信息的枚举。</param>
        /// <param name="ignoreDefaultDisplayName">指定是否忽略默认显示名称的枚举字段。</param>
        /// <returns>描述 <paramref name="source" /> 中所有枚举字段信息的 <see cref="T:YuLinTu.EnumField" /> 数组。</returns>
        public static EnumField[] GetFields(this Enum source, bool ignoreDefaultDisplayName = false)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return EnumField.GetFields(source.GetType(), ignoreDefaultDisplayName);
        }
    }
}
