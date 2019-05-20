using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 提供一组 <see cref="T:System.String" /> 对象的常用 static（在 Visual Basic 中为 Shared）方法。
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 指示指定的 <see cref="T:System.String" /> 对象是 null 还是 <see cref="F:System.String.Empty" /> 字符串。
        /// </summary>
        /// <param name="source">要测试的字符串。</param>
        /// <returns>如果要测试的字符串为 null 或空字符串 ("")，则为 true；否则为 false。</returns>
        /// <remarks>当对 null 的字符串以扩展方法方式调用此方法时不会引发 <see cref="T:System.NullReferenceException" /> 异常。</remarks>
        /// <example>
        ///     <code>
        ///         void Test(string name) {
        ///             if(name.IsNullOrEmpty())
        ///                 throw new ArgumentNullException("name");
        ///
        ///             Console.WriteLine(name);
        ///         }
        ///     </code>
        ///     等同于
        ///     <code>
        ///         void Test(string name) {
        ///             if(String.IsNullOrEmpty(name))
        ///                 throw new ArgumentNullException("name");
        ///
        ///             Console.WriteLine(name);
        ///         }
        ///     </code>
        /// </example>
        public static bool IsNullOrEmpty(this string source)
        {
            return string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 指示指定的 <see cref="T:System.String" /> 对象即不为是 null 也不是 <see cref="F:System.String.Empty" /> 字符串。
        /// </summary>
        /// <param name="source">要测试的字符串。</param>
        /// <returns>如果要测试的字符串不为 null 也不是空字符串 ("")，则为 true；否则为 false。</returns>
        /// <remarks>当对 null 的字符串以扩展方法方式调用此方法时不会引发 <see cref="T:System.NullReferenceException" /> 异常。</remarks>
        public static bool IsNotNullOrEmpty(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="source">要测试的字符串。</param>
        /// <returns>如果 <paramref name="source" /> 参数为 <c>null</c> 或 <see cref="F:System.String.Empty" />，或者如果 <paramref name="source" /> 仅由空白字符组成，则为 <c>true</c>。</returns>
        /// <remarks>当对 null 的字符串以扩展方法方式调用此方法时不会引发 <see cref="T:System.NullReferenceException" /> 异常。</remarks>
        public static bool IsNullOrWhiteSpace(this string source)
        {
            return string.IsNullOrWhiteSpace(source);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        /// <param name="source">要测试的字符串。</param>
        /// <returns>如果 <paramref name="source" /> 参数为 <c>null</c> 或 <see cref="F:System.String.Empty" />，或者如果 <paramref name="source" /> 仅由空白字符组成，则为 <c>true</c>。</returns>
        /// <remarks>当对 null 的字符串以扩展方法方式调用此方法时不会引发 <see cref="T:System.NullReferenceException" /> 异常。</remarks>
        public static bool IsNotNullOrWhiteSpace(this string source)
        {
            return !source.IsNullOrWhiteSpace();
        }

        /// <summary>
        /// 检测字符串如果是 <c>null</c>或空字符串就返回指定的默认值 <paramref name="defaultValue" />，否则返回原始值。
        /// </summary>
        /// <param name="source">要检测的字符串。</param>
        /// <param name="defaultValue">默认值。</param>
        /// <returns>如果检测的字符串是 <c>null</c>或空字符串就返回指定的默认值 <paramref name="defaultValue" />，否则返回原始值。</returns>
        public static string IfNullOrEmtpy(this string source, string defaultValue)
        {
            if (!string.IsNullOrEmpty(source))
            {
                return source;
            }
            return defaultValue;
        }

        /// <summary>
        /// 检测字符串如果是 <c>null</c>、空字符串或仅由空白字符组成就返回指定的默认值 <paramref name="defaultValue" />，否则返回原始值。
        /// </summary>
        /// <param name="source">要检测的字符串。</param>
        /// <param name="defaultValue">指定默认值。</param>
        /// <returns>如果检测的字符串是 <c>null</c>、空字符串或仅由空白字符组成则返回指定的默认字符串，否则返回原始字符串。</returns>
        public static string IfNullOrWhiteSpace(this string source, string defaultValue)
        {
            if (!source.IsNullOrWhiteSpace())
            {
                return source;
            }
            return defaultValue;
        }

        /// <summary>
        /// 清空字符串中的所有空白字符（包含空格、换行符和制表符）。
        /// </summary>
        /// <param name="source">要进行空白字符清空操作的字符串。</param>
        /// <returns>清空所有空白字符后的字符串。</returns>
        public static string ClearWhiteSpace(this string source)
        {
            if (source.IsNullOrWhiteSpace())
            {
                return string.Empty;
            }
            return source.Replace("\\s", string.Empty);
        }

        /// <summary>
        /// 清除字符串中所有的特殊字符。
        /// </summary>
        /// <param name="value">输入的字符串。</param>
        /// <returns>清除所有特殊字符后的字符串，只包含大小英文字母及数字。</returns>
        public static string ClearAllSpecialCharacters(this string value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (char value2 in from c in value
                                    where (c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z')
                                    select c)
            {
                stringBuilder.Append(value2);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 从此实例的左边获取指定长度的子字符串。
        /// </summary>
        /// <param name="source">要从中获取子字符串的字符串实例。</param>
        /// <param name="length">要获取的字符串的长度。</param>
        /// <returns>一个 <see cref="T:System.String" />，它等于此实例中从左边起始位置开始的长度为 <paramref name="length" /> 的子字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        public static string Left(this string source, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source.Length >= length)
            {
                return source.Substring(0, length);
            }
            return source;
        }

        /// <summary>
        /// 从此实例的右边获取指定长度的子字符串。
        /// </summary>
        /// <param name="source">要从中获取子字符串的字符串实例。</param>
        /// <param name="length">要获取的字符串的长度。</param>
        /// <returns>一个 <see cref="T:System.String" />，它等于此实例中从最右边开始往左边计数的长度为 <paramref name="length" /> 的子字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        public static string Right(this string source, int length)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (source.Length >= length)
            {
                return source.Substring(source.Length - length);
            }
            return source;
        }

        /// <summary>
        /// 确定此实例是否与另一个指定的 <see cref="T:System.String" /> 对象具有相同的值，但将忽略大小写。
        /// </summary>
        /// <param name="source">要与 <paramref name="value" /> 判断值相同的字符串实例。</param>
        /// <param name="value"><see cref="T:System.String" />。</param>
        /// <returns>如果 <paramref name="value" /> 参数的值与此实例相同（忽略大小写），则为 <c>true</c>；否则为 <c>false</c>。</returns>
        public static bool EqualsIgnoreCase(this string source, string value)
        {
            return (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(value)) || (!string.IsNullOrEmpty(source) && !string.IsNullOrEmpty(value) && source.Length == value.Length && 0 == string.Compare(source, 0, value, 0, value.Length, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// 截取字符串使其长度不能超过指定的最大值。
        /// </summary>
        /// <param name="source">输入用于截取的字符串。</param>
        /// <param name="maxLength">截取获得的字符串的最大长度。</param>
        /// <returns>如果输入的字符串长度超过给定的最大长度则对字符串进行截取并返回截取值，否则返回原字符串。</returns>
        public static string TrimToMaxLength(this string source, int maxLength)
        {
            if (source != null && source.Length > maxLength)
            {
                return source.Substring(0, maxLength);
            }
            return source;
        }

        /// <summary>
        /// 截取字符串使其长度不能超过指定的最大值并在结果字符最后附加一个指定的字符串。
        /// </summary>
        /// <param name="source">输入用于截取的字符串。</param>
        /// <param name="maxLength">截取获得的字符串的最大长度。</param>
        /// <param name="suffix">用于附加到截取结果字符串后的 <see cref="T:System.String" /> 实例。</param>
        /// <returns>如果输入的字符串长度超过给定的最大长度则对字符串进行截取并须结尾处附加 <paramref name="suffix" /> 后作为返回值，否则返回原字符串。</returns>
        /// <example>
        ///     <code>
        ///         "abcdefghijklm".TrimToMaxLenth(5, "...");
        ///     </code>
        /// </example>
        public static string TrimToMaxLength(this string source, int maxLength, string suffix)
        {
            if (source != null && source.Length > maxLength)
            {
                return source.Substring(0, maxLength) + suffix;
            }
            return source;
        }

        /// <summary>
        /// 确保字符串以指定的字符串开头。
        /// </summary>
        /// <param name="source">输入的字符串。</param>
        /// <param name="prefix">起始字符串。</param>
        /// <returns>如果字符串 <paramref name="prefix" /> 开头则返回原字符串，否则将在输入字符串的开头处追加 <paramref name="prefix" /> 并返回。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <example>
        ///     <code>
        ///         string extension = "txt";
        ///         string fileName = String.Concat(file.Name, extension.EnsureStartsWith("."));
        ///     </code>
        /// </example>
        public static string EnsureStartsWith(this string source, string prefix)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!source.StartsWith(prefix))
            {
                return prefix + source;
            }
            return source;
        }

        /// <summary>
        /// 确保字符串以指定的字符串结尾。
        /// </summary>
        /// <param name="source">输入的字符串。</param>
        /// <param name="suffix">字符串的结尾。</param>
        /// <returns>如果字符串以 <paramref name="suffix" /> 结尾则返回原字符串，否则在字符串的结尾处追加 <paramref name="suffix" /> 并返回。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <example>
        ///     <code>
        ///         string url = "http://www.yulintu.com";
        ///         url = url.EnsureEndsWith("/");
        ///     </code>
        /// </example>
        public static string EnsureEndsWith(this string source, string suffix)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (!source.EndsWith(suffix))
            {
                return source + suffix;
            }
            return source;
        }

        /// <summary>
        /// 确保字串不为 <c>null</c>。
        /// </summary>
        /// <param name="source">输入的字符串。</param>
        /// <returns><see cref="T:System.String" />。如果字符输入的字符串为 <c>null</c> 则返回空字符串，否则返回输入的字符串。</returns>
        public static string EnsureHasValue(this string source)
        {
            if (source != null)
            {
                return source;
            }
            return string.Empty;
        }

        /// <summary>
        /// 把字符串转换成指定类型且值与字符串所表示的值等效的值。
        /// </summary>
        /// <typeparam name="T">字符串要转换的目标类型，必须是值类型。</typeparam>
        /// <param name="source">要进行转换的字符串值。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的对象，它的值等效于字符串所描述的值。如果转换失败则返回 <typeparamref name="T" /> 类型的默认值。</returns>
        public static T To<T>(this string source)
        {
            return source.To(default(T));
        }

        /// <summary>
        /// 把字符串转换成指定类型且值与字符串所表示的值等效的值。
        /// </summary>
        /// <typeparam name="T">字符串要转换的目标类型，必须是值类型。</typeparam>
        /// <param name="source">要进行转换的字符串值。</param>
        /// <param name="defaultValue">当转换失败时返回的默认值。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的对象，它的值等效于字符串所描述的值。如果字符串为 <c>null</c> 或仅由空白字符组成或转换失败则返回 <paramref name="defaultValue" />。</returns>
        public static T To<T>(this string source, T defaultValue)
        {
            if (source.IsNullOrWhiteSpace())
            {
                return defaultValue;
            }
            T result = defaultValue;
            try
            {
                Type typeFromHandle = typeof(T);
                if (typeFromHandle == typeof(Guid))
                {
                    result = (T)((object)new Guid(source));
                }
                else if (typeFromHandle.BaseType == typeof(Enum))
                {
                    result = (T)((object)Enum.Parse(typeFromHandle, source));
                }
                else
                {
                    result = (T)((object)Convert.ChangeType(source, typeFromHandle));
                }
            }
            catch
            {
            }
            return result;
        }

        /// <summary>
        /// 获取字符串中在 <paramref name="x" /> 字符串之前的字符串。
        /// </summary>
        /// <param name="value">输入用于截取的字符串。</param>
        /// <param name="x">用于判断截取的字符串。</param>
        /// <returns>返回字符串实例中在 <paramref name="x" /> 之前的字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value" /> 或 <paramref name="x" /> 为空。</exception>
        public static string GetBefore(this string value, string x)
        {
            if (value == null || x == null)
            {
                throw new ArgumentNullException((value == null) ? "value" : "x");
            }
            int num = value.IndexOf(x);
            if (num != -1)
            {
                return value.Substring(0, num);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字符串在 <paramref name="x" /> 字符串之后的字符串。
        /// </summary>
        /// <param name="value">输入用于截取的字符串。</param>
        /// <param name="x">用于判断截取的字符串。</param>
        /// <returns>返回字符串实例中在 <paramref name="x" /> 之后的字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value" /> 或 <paramref name="x" /> 为空。</exception>
        public static string GetAfter(this string value, string x)
        {
            if (value == null || x == null)
            {
                throw new ArgumentNullException((value == null) ? "value" : "x");
            }
            int num = value.LastIndexOf(x);
            if (num == -1)
            {
                return string.Empty;
            }
            int num2 = num + x.Length;
            if (num2 < value.Length)
            {
                return value.Substring(num2);
            }
            return string.Empty;
        }

        /// <summary>
        /// 获取字符串中介于 <paramref name="x" /> 与 <paramref name="y" /> 之间的字符串。
        /// </summary>
        /// <param name="value">输入用于截取的字符串。</param>
        /// <param name="x">用于判断截取开始的字符串。</param>
        /// <param name="y">用于判断截取结束的字串串。</param>
        /// <returns>返回介于 <paramref name="x" /> 与 <paramref name="y" /> 之间的字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="value" />、<paramref name="x" /> 或 <paramref name="y" /> 为空。</exception>
        public static string GetBetween(this string value, string x, string y)
        {
            if (value == null || x == null)
            {
                throw new ArgumentNullException((value == null) ? "value" : "x");
            }
            if (y == null)
            {
                throw new ArgumentNullException("y");
            }
            int num = value.IndexOf(x);
            int num2 = value.LastIndexOf(y);
            if (num == -1 || num2 == -1)
            {
                return string.Empty;
            }
            int num3 = num + x.Length;
            if (num3 < num2)
            {
                return value.Substring(num3, num2 - num3);
            }
            return string.Empty;
        }

        /// <summary>
        /// 将字符串以默认编码格式编码为一个字节序列。
        /// </summary>
        /// <param name="source">要进行编码的字符串。</param>
        /// <returns>一个字节数组，包含对指定的字符集进行编码的结果。</returns>
        public static byte[] ToBytes(this string source)
        {
            return source.ToBytes(null);
        }

        /// <summary>
        /// 将字符串以指定的编码格式编码为一个字节序列。
        /// </summary>
        /// <param name="source">要进行编码的字符串</param>
        /// <param name="encoding">对字符串进行编码的编码格式，如果为 <c>null</c> 则使用 <see cref="P:System.Text.Encoding.Default" /> 作为默认值。</param>
        /// <returns>一个字节数组，包含对指定的字符集进行编码的结果。</returns>
        public static byte[] ToBytes(this string source, Encoding encoding)
        {
            if (source == null)
            {
                return null;
            }
            encoding = (encoding ?? Encoding.Default);
            return encoding.GetBytes(source);
        }

        /// <summary>
        /// 将指定的 <see cref="T:System.String" /> 中的格式项替换为指定的 <see cref="T:System.Object" /> 实例的值的文本等效项。
        /// </summary>
        /// <param name="source"><see cref="T:System.String" />，包含零个或多个格式项。</param>
        /// <param name="arg0">要格式化的 <see cref="T:System.Object" />。</param>
        /// <returns>当前字符串的一个副本，其中的第一个格式项已替换为 <paramref name="arg0" /> 的 <see cref="T:System.String" /> 等效项。</returns>
        public static string FormatWith(this string source, object arg0)
        {
            return string.Format(source, arg0);
        }

        /// <summary>
        /// 将指定的 <see cref="T:System.String" /> 中的格式项替换为两个指定的 <see cref="T:System.Object" /> 实例的值的文本等效项。
        /// </summary>
        /// <param name="source"><see cref="T:System.String" />，包含零个或多个格式项。</param>
        /// <param name="arg0">第一个要格式化的 <see cref="T:System.Object" />。</param>
        /// <param name="arg1">第二个要格式化的 <see cref="T:System.Object" />。</param>
        /// <returns>当前字符串的一个副本，其中的第一个和第二个格式项已替换为 <paramref name="arg0" /> 和 <paramref name="arg1" /> 的 <see cref="T:System.String" /> 等效项。</returns>
        public static string FormatWith(this string source, object arg0, object arg1)
        {
            return string.Format(source, arg0, arg1);
        }

        /// <summary>
        /// 将指定的 <see cref="T:System.String" /> 中的格式项替换为三个指定的 <see cref="T:System.Object" /> 实例的值的文本等效项。
        /// </summary>
        /// <param name="source"><see cref="T:System.String" />，包含零个或多个格式项。</param>
        /// <param name="arg0">第一个要格式化的 <see cref="T:System.Object" />。</param>
        /// <param name="arg1">第二个要格式化的 <see cref="T:System.Object" />。</param>
        /// <param name="arg2">第三个要格式化的 <see cref="T:System.Object" />。</param>
        /// <returns>当前字符串的一个副本，其中的第一个、第二个和第三个格式项已替换为 <paramref name="arg0" />、<paramref name="arg1" /> 和 <paramref name="arg2" /> 的 <see cref="T:System.String" /> 等效项。</returns>
        public static string FormatWith(this string source, object arg0, object arg1, object arg2)
        {
            return string.Format(source, arg0, arg1, arg2);
        }

        /// <summary>
        /// 将指定 <see cref="T:System.String" /> 中的格式项替换为指定数组中相应 <see cref="T:System.Object" /> 实例的值的文本等效项。
        /// </summary>
        /// <param name="source"><see cref="T:System.String" />，包含零个或多个格式项。</param>
        /// <param name="args">包含零个或多个要格式化的对象的 <see cref="T:System.Object" /> 数组。</param>
        /// <returns>当前字符串的一个副本，其中格式项已替换为 <paramref name="args" /> 中相应 <see cref="T:System.Object" /> 实例的 <see cref="T:System.String" /> 等效项。</returns>
        public static string FormatWith(this string source, params object[] args)
        {
            return string.Format(source, args);
        }

        /// <summary>
        /// 确定指定的字符串是否是 Base 64 字符串。
        /// </summary>
        /// <param name="source">输入的字符串。</param>
        /// <returns>如果是 Base 64 字符串则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        public static bool IsBase64String(this string source)
        {
            return Regex.IsMatch(source, "[A-Za-z0-9\\+\\/\\=]", RegexOptions.Compiled);
        }

        /// <summary>
        /// 将字符串转换为其等效的、用以 64 为基的数字编码的 <see cref="T:System.String" /> 表示形式。
        /// </summary>
        /// <param name="source">要转换的字符串。</param>
        /// <returns>此字符串以 base 64 表示的 <see cref="T:System.String" /> 表示形式。</returns>
        public static string EncodeToBase64(this string source)
        {
            return source.EncodeToBase64(null, Base64FormattingOptions.None);
        }

        /// <summary>
        /// 将字符串转换为其等效的、用以 64 为基的数字编码的 <see cref="T:System.String" /> 表示形式。
        /// </summary>
        /// <param name="source">要转换的字符串。</param>
        /// <param name="encoding">指定当前字符中的编码格式。</param>
        /// <returns>此字符串以 base 64 表示的 <see cref="T:System.String" /> 表示形式。</returns>
        public static string EncodeToBase64(this string source, Encoding encoding)
        {
            return source.EncodeToBase64(encoding, Base64FormattingOptions.None);
        }

        /// <summary>
        /// 将字符串转换为其等效的、用以 64 为基的数字编码的 <see cref="T:System.String" /> 表示形式。参数指定字符串的编码格式及是否在返回值中插入分行符。
        /// </summary>
        /// <param name="source">要转换的字符串。</param>
        /// <param name="encoding">指定当前字符中的编码格式。</param>
        /// <param name="options">如果每 76 个字符插入一个分行符，则使用 <see cref="F:System.Base64FormattingOptions.InsertLineBreaks" />，如果不插入分行符，则使用 <see cref="F:System.Base64FormattingOptions.None" />。</param>
        /// <returns>此字符串以 base 64 表示的 <see cref="T:System.String" /> 表示形式。</returns>
        public static string EncodeToBase64(this string source, Encoding encoding, Base64FormattingOptions options)
        {
            encoding = (encoding ?? Encoding.UTF8);
            byte[] bytes = encoding.GetBytes(source);
            return Convert.ToBase64String(bytes, options);
        }

        /// <summary>
        /// 将指定的 <see cref="T:System.String" />（它将二进制数据编码为 base 64 数字）转换成等效的普通字符串。
        /// </summary>
        /// <param name="source"><see cref="T:System.String" />。</param>
        /// <returns>当前二进制数据编码为 base 64 数字的字符串的普通字符串表达形式。</returns>
        public static string DecodeBase64(this string source)
        {
            return source.DecodeBase64(null);
        }

        /// <summary>
        /// 将指定的 <see cref="T:System.String" />（它将二进制数据编码为 base 64 数字）转换成等效的普通字符串。
        /// </summary>
        /// <param name="source"><see cref="T:System.String" />。</param>
        /// <param name="encoding">目标字符串的编码格式。</param>
        /// <returns>当前二进制数据编码为 base 64 数字的字符串的普通字符串表达形式（由 <paramref name="encoding" /> 指定字符串的编码格式）。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        public static string DecodeBase64(this string source, Encoding encoding)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            encoding = (encoding ?? Encoding.UTF8);
            byte[] bytes = Convert.FromBase64String(source);
            return encoding.GetString(bytes);
        }

        /// <summary>
        /// 报告指定的字符串 <paramref name="value" /> 是否在当前 <see cref="T:System.String" /> 对象中存在。一个参数指定要用于指定字符串的搜索类型。
        /// </summary>
        /// <param name="source">要搜索匹配项的字符串。</param>
        /// <param name="value">要匹配的字符串。</param>
        /// <param name="ignoreCase"><see cref="T:System.Boolean" />，指示所进行的比较是否区分大小写。（<c>true</c> 指示所进行的比较不区分大小写。）</param>
        /// <returns>如果找到该字符串，则返回 <c>true</c>；如果未找到该字符串，则为 <c>false</c>。如果 <paramref name="value" /> 为 <see cref="F:System.String.Empty" />，则返回值为 <c>true</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        public static bool Contains(this string source, string value, bool ignoreCase)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (ignoreCase)
            {
                return source.Contains(value, StringComparison.CurrentCultureIgnoreCase);
            }
            return source.Contains(value);
        }

        /// <summary>
        /// 报告指定的字符串 <paramref name="value" /> 是否在当前 <see cref="T:System.String" /> 对象中存在。一个参数指定要用于指定字符串的搜索类型。
        /// </summary>
        /// <param name="source">要搜索匹配项的字符串。</param>
        /// <param name="value">要匹配的字符串。</param>
        /// <param name="comparisonType"><see cref="T:System.StringComparison" /> 值之一。</param>
        /// <returns>如果找到该字符串，则返回 <c>true</c>；如果未找到该字符串，则为 <c>false</c>。如果 <paramref name="value" /> 为 <see cref="F:System.String.Empty" />，则返回值为 <c>true</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="value" /> 为 <c>null</c>。</exception>
        public static bool Contains(this string source, string value, StringComparison comparisonType)
        {
            if (source == null || value == null)
            {
                throw new ArgumentNullException((source == null) ? "source" : "value");
            }
            return source.IndexOf(value, comparisonType) >= 0;
        }

        /// <summary>
        /// 指示正则表达式使用 <paramref name="pattern" /> 参数中指定的正则表达式是否在输入字符串中找到匹配项。
        /// </summary>
        /// <param name="source">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <returns>如果正则表达式找到匹配项，则为 <c>true</c>；否则，为 <c>false</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="pattern" /> 为 <c>null</c>。</exception>
        /// <seealso cref="M:System.Text.RegularExpressions.Regex.IsMatch(System.String,System.String)" />
        public static bool IsMatch(this string source, string pattern)
        {
            if (source == null || pattern == null)
            {
                throw new ArgumentNullException((source == null) ? "source" : "pattern");
            }
            return Regex.IsMatch(source, pattern);
        }

        /// <summary>
        /// 指示正则表达式使用 <paramref name="pattern" /> 参数中指定的正则表达式是否在输入字符串中找到匹配项。
        /// </summary>
        /// <param name="source">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="options"><see cref="T:System.Text.RegularExpressions.RegexOptions" /> 枚举值的按位“或”组合。</param>
        /// <returns>如果正则表达式找到匹配项，则为 <c>true</c>；否则，为 <c>false</c>。</returns>
        /// <seealso cref="M:System.Text.RegularExpressions.Regex.IsMatch(System.String,System.String,System.Text.RegularExpressions.RegexOptions)" />
        public static bool IsMatch(this string source, string pattern, RegexOptions options)
        {
            return Regex.IsMatch(source, pattern, options);
        }

        /// <summary>
        /// 在指定的输入字符串中搜索 <paramref name="pattern" /> 参数中提供的正则表达式的匹配项。
        /// </summary>
        /// <param name="source">要搜索匹配项的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <returns>与正则表达式模式 <paramref name="pattern" /> 匹配的字符串对象，如果未找到匹配项则返回 <see cref="F:System.String.Empty" />。</returns>
        /// <seealso cref="M:System.Text.RegularExpressions.Regex.Match(System.String,System.String)" />
        public static string Match(this string source, string pattern)
        {
            if (source == null)
            {
                return string.Empty;
            }
            return Regex.Match(source, pattern).Value;
        }

        /// <summary>
        /// 从字符串中的第一个字符开始，用替换字符串替换由正则表达式定义的匹配的所有匹配项。
        /// </summary>
        /// <param name="source">要修改的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="replacement">替换字符串。</param>
        /// <returns>已修改的字符串。</returns>
        /// <seealso cref="M:System.Text.RegularExpressions.Regex.Replace(System.String,System.String,System.String)" />
        public static string Replace(this string source, string pattern, string replacement)
        {
            return Regex.Replace(source, pattern, replacement);
        }

        /// <summary>
        /// 从字符串中的第一个字符开始，用替换字符串替换由正则表达式定义的匹配的所有匹配项。
        /// </summary>
        /// <param name="source">要修改的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="replacement">替换字符串。</param>
        /// <param name="options"><see cref="T:System.Text.RegularExpressions.RegexOptions" /> 枚举值的按位“或”组合。</param>
        /// <returns>已修改的字符串。</returns>
        /// <see cref="M:System.Text.RegularExpressions.Regex.Replace(System.String,System.String,System.String,System.Text.RegularExpressions.RegexOptions)" />
        public static string Replace(this string source, string pattern, string replacement, RegexOptions options)
        {
            return Regex.Replace(source, pattern, replacement, options);
        }

        /// <summary>
        /// 从第一个字符开始，用替换字符串替换由正则表达式定义的字符模式的所有匹配项。在每个匹配处均调用 <see cref="T:System.Text.RegularExpressions.MatchEvaluator" /> 委托以计算替换。
        /// </summary>
        /// <param name="source">要修改的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="evaluator">在每一步计算替换的 <see cref="T:System.Text.RegularExpressions.MatchEvaluator" />。</param>
        /// <returns>已修改的字符串。</returns>
        /// <see cref="M:System.Text.RegularExpressions.Regex.Replace(System.String,System.String,System.Text.RegularExpressions.MatchEvaluator)" />
        public static string Replace(this string source, string pattern, MatchEvaluator evaluator)
        {
            return Regex.Replace(source, pattern, evaluator);
        }

        /// <summary>
        /// 从第一个字符开始，用替换字符串替换由正则表达式定义的字符模式的所有匹配项。在每个匹配处均调用 <see cref="T:System.Text.RegularExpressions.MatchEvaluator" /> 委托以计算替换。
        /// </summary>
        /// <param name="source">要修改的字符串。</param>
        /// <param name="pattern">要匹配的正则表达式模式。</param>
        /// <param name="evaluator">在每一步计算替换的 <see cref="T:System.Text.RegularExpressions.MatchEvaluator" />。</param>
        /// <param name="options"><see cref="T:System.Text.RegularExpressions.RegexOptions" /> 枚举值的按位“或”组合。</param>
        /// <returns>已修改的字符串。</returns>
        /// <seealso cref="M:System.Text.RegularExpressions.Regex.Replace(System.String,System.String,System.Text.RegularExpressions.MatchEvaluator,System.Text.RegularExpressions.RegexOptions)" />
        public static string Replace(this string source, string pattern, MatchEvaluator evaluator, RegexOptions options)
        {
            return Regex.Replace(source, pattern, evaluator, options);
        }

        /// <summary>
        /// 将当前字符串转换成应加密的字符串。
        /// </summary>
        /// <param name="source">要转换的字符串。</param>
        /// <param name="makeReadOnly">是否将转换后的加密字符串标识的只读。</param>
        /// <returns>转换后的加密字符串。</returns>
        public static SecureString ToSecureString(this string source, bool makeReadOnly = true)
        {
            if (source == null)
            {
                return null;
            }
            SecureString secureString = new SecureString();
            foreach (char c in source)
            {
                secureString.AppendChar(c);
            }
            if (makeReadOnly)
            {
                secureString.MakeReadOnly();
            }
            return secureString;
        }

        /// <summary>
        /// 将当前 <see cref="T:System.Security.SecureString" /> 转换成普通字符串。
        /// </summary>
        /// <param name="source">要转换的 <see cref="T:System.Security.SecureString" /> 对象。</param>
        /// <returns>返回与 <paramref name="source" /> 对应的普通字符串。</returns>
        public static string ToString(this SecureString source)
        {
            if (source == null)
            {
                return null;
            }
            IntPtr intPtr = IntPtr.Zero;
            string result;
            try
            {
                intPtr = Marshal.SecureStringToGlobalAllocUnicode(source);
                result = Marshal.PtrToStringUni(intPtr);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(intPtr);
            }
            return result;
        }
    }
}
