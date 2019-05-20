using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 提供一组 <see cref="T:System.Object" /> 对象的常用 static（在 Visual Basic 中为 Shared）方法。
    /// </summary>
    // Token: 0x02000056 RID: 86
    public static class ObjectExtensions
    {
        /// <summary>
        /// 将指定的 <see cref="T:System.String" /> 中的格式项替换为 <paramref name="source" /> 实例的属性或方法返回值的文本等效项。
        /// </summary>
        /// <param name="source">包含要进行转换的信息内容的对象。</param>
        /// <param name="format">包含零个或多个对象属性的格式项。</param>
        /// <param name="args">包含零个或多个用于参与格式化对象调用的 <see cref="T:System.Object" /> 参数数组。</param>
        /// <returns>此实例信息的字符串表示形式，由 <paramref name="format" /> 指定。</returns>
        /// <remarks>此方法在格式字符串中使用方法并自带参数时，运行时只把参数作为处理；且格式字符串中只支持1维数组的调用。</remarks>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <example>
        ///     <code>
        ///         var per = new{Name = "Roc",Age = 23,Members = new[]{new{Name="Dragon",Age=32},new{Name="Jason",Age=18}}};
        ///         string str = obj.ToString("Name:{name},Name.SubString(@0,@0):{name.substring(@0,@0)},Age:{age},Members[0].Name:{Members[0].Name}", 1);
        ///     </code>
        /// </example>
        public static string ToString(this object source, string format, params object[] args)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(format))
            {
                return source.ToString();
            }
            MatchEvaluator evaluator = delegate (Match match)
            {
                string[] array = match.Groups["Name"].Value.Split(new char[]
                {
                    '.'
                });
                string value = match.Groups["Format"].Value;
                object obj = source;
                try
                {
                    foreach (string text in array)
                    {
                        if (text.Contains("("))
                        {
                            obj = ObjectExtensions.InvokeFormaterMethod(obj, text, args);
                        }
                        else if (text.StartsWith("["))
                        {
                            obj = ObjectExtensions.InvokeFormaterIndexer(obj, text, args);
                        }
                        else if (text.Contains("["))
                        {
                            obj = ObjectExtensions.InvokeFormatterArrayIndexer(obj, text, new object[0]);
                        }
                        else
                        {
                            obj = obj.GetPropertyValue(text);
                        }
                    }
                    if (obj == null)
                    {
                        return match.Value;
                    }
                }
                catch
                {
                    return match.Value;
                }
                if (string.IsNullOrEmpty(value))
                {
                    return obj.ToString();
                }
                if (!value.StartsWith("."))
                {
                    return string.Format("{0:" + value + "}", obj);
                }
                string subPropertyName = value.Substring(1);
                IEnumerable<object> source2 = ((IEnumerable)obj).Cast<object>();
                if (subPropertyName == "Count")
                {
                    return source2.Count<object>().ToString();
                }
                string[] value2 = (from o in source2
                                   select o.GetPropertyValue(subPropertyName).ToString()).ToArray<string>();
                return string.Join(", ", value2);
            };
            string pattern = "\\{(?<Name>[^\\{\\}:]+)(\\s*[:]\\s*(?<Format>[^\\{\\}:]+))?\\}";
            return Regex.Replace(format, pattern, evaluator, RegexOptions.Compiled);
        }

        /// <summary>
        /// 将对象序列化成 Xml 字符串。
        /// </summary>
        /// <param name="source">对象实例。</param>
        /// <returns>表示对象信息的 Xml 字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为空。</exception>
        // Token: 0x0600041A RID: 1050 RVA: 0x0000F0E0 File Offset: 0x0000D2E0
        public static string ToXmlString(this object source)
        {
            return source.ToXmlString(true);
        }

        /// <summary>
        /// 将对象序列化成 <see cref="T:System.Xml.Linq.XDocument" />。
        /// </summary>
        /// <param name="source">要序列化成 <see cref="T:System.Xml.Linq.XDocument" /> 的对象实例。</param>
        /// <returns>表示 XML 文档的 <see cref="T:System.Xml.Linq.XDocument" />。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为空。</exception>
        public static XDocument ToXDocument(this object source)
        {
            if (source == null)
            {
                return null;
            }
            XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
            XDocument result;
            using (Stream stream = new MemoryStream())
            {
                xmlSerializer.Serialize(stream, source);
                stream.Position = 0L;
                result = XDocument.Load(stream);
            }
            return result;
        }

        /// <summary>
        /// 将对象序列化成 <see cref="T:System.Xml.Linq.XElement" />。
        /// </summary>
        /// <param name="source">要序列化成 <see cref="T:System.Xml.Linq.XElement" /> 的对象实例。</param>
        /// <returns>表示 Xml 元素的 <see cref="T:System.Xml.Linq.XElement" />。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为空。</exception>
        public static XElement ToXElement(this object source)
        {
            if (source == null)
            {
                return null;
            }
            return source.ToXDocument().Root;
        }

        /// <summary>
        /// 将对象序列化成 Xml 字符串。
        /// </summary>
        /// <param name="source">要序列化成 Xml 字符串的对象实例。</param>
        /// <param name="containsDeclaration">指定是否包含文档声明及命名空间声明。</param>
        /// <returns>表示对象信息的 Xml 字符串。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为空。</exception>
        public static string ToXmlString(this object source, bool containsDeclaration)
        {
            if (source == null)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, new XmlWriterSettings
            {
                OmitXmlDeclaration = !containsDeclaration
            }))
            {
                XmlSerializer xmlSerializer = new XmlSerializer(source.GetType());
                xmlSerializer.Serialize(xmlWriter, source);
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// 将对象序列化成二进制格式。
        /// </summary>
        /// <param name="source">要序列化成二进制格式的对象实例。</param>
        /// <returns>将对象序列化成二进制格式后的 <see cref="T:System.Byte" /> 数组。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.Runtime.Serialization.SerializationException">序列化期间发生错误，如 <paramref name="source" /> 参数中的某个对象未标记为可序列化。</exception>
        /// <exception cref="T:System.Security.SecurityException">调用方没有所要求的权限。</exception>
        /// <remarks>
        /// 要序列化的对象或对象中包含的对象必须使用 <see cref="T:System.SerializableAttribute" /> 自定义属性标记为可序列化。
        /// </remarks>
        /// <history>
        /// 2011-03-21 16:09:16 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600041E RID: 1054 RVA: 0x0000F1C0 File Offset: 0x0000D3C0
        public static byte[] ToBinary(this object source)
        {
            if (source == null)
            {
                return null;
            }
            byte[] result;
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter binaryFormatter = new BinaryFormatter();
                binaryFormatter.Serialize(memoryStream, source);
                result = memoryStream.ToArray();
            }
            return result;
        }

        /// <summary>
        /// 调用实例中指定名称的方法或构造函数。
        /// </summary>
        /// <param name="source">对其调用方法的对象。</param>
        /// <param name="methodName">方法的名称。</param>
        /// <param name="parameters">
        /// 调用的方法或构造函数的参数列表。这是一个对象数组，这些对象与要调用的方法或构造函数的参数具有相同的数量、顺序和类型。如果没有任何参数，则 <paramref name="parameters" />
        /// 应为 <c>null</c>。如果此实例所表示的方法或构造函数采用 <c>ref</c> 参数（在 Visual Basic 中为 ByRef），使用此函数调用该方法或构造函数时，该参数不需要任何特殊属性。如果数组中的对象未用值来显式初始化，则该对象将包含该对象类型的默认值。对于引用类型的元素，该值为
        /// <c>null</c>。对于值类型的元素，该值为 0、0.0 或 <c>false</c>，具体取决于特定的元素类型。
        /// </param>
        /// <returns>一个对象，包含被调用方法的返回值。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="methodName" /> 为空。</exception>
        /// <exception cref="T:System.ArgumentException">未找到 <paramref name="methodName" /> 指定的方法。</exception>
        /// <history>
        ///     2011-02-15 17:15:37 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600041F RID: 1055 RVA: 0x0000F20C File Offset: 0x0000D40C
        public static object InvokeMethod(this object source, string methodName, params object[] parameters)
        {
            return source.InvokeMethod(methodName, parameters);
        }

        /// <summary>
        /// 调用实例中指定名称的方法或构造函数。
        /// </summary>
        /// <typeparam name="T">方法的返回值类型。</typeparam>
        /// <param name="source">对其调用方法的对象。</param>
        /// <param name="methodName">方法的名称。</param>
        /// <param name="parameters">
        /// 调用的方法或构造函数的参数列表。这是一个对象数组，这些对象与要调用的方法或构造函数的参数具有相同的数量、顺序和类型。如果没有任何参数，则 <paramref name="parameters" />
        /// 应为 <c>null</c>。如果此实例所表示的方法或构造函数采用 <c>ref</c> 参数（在 Visual Basic 中为 ByRef），使用此函数调用该方法或构造函数时，该参数不需要任何特殊属性。如果数组中的对象未用值来显式初始化，则该对象将包含该对象类型的默认值。对于引用类型的元素，该值为
        /// <c>null</c>。对于值类型的元素，该值为 0、0.0 或 <c>false</c>，具体取决于特定的元素类型。
        /// </param>
        /// <returns>一个 <typeparamref name="T" /> 类型的对象，包含被调用方法的返回值，如果方法返回值的类型不是 <typeparamref name="T" /> 则返回 <typeparamref name="T" /> 的类型默认值。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="methodName" /> 为空。</exception>
        /// <exception cref="T:System.ArgumentException">未找到 <paramref name="methodName" /> 指定的方法。</exception>
        /// <history>
        ///     2011-02-15 17:15:24 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000420 RID: 1056 RVA: 0x0000F218 File Offset: 0x0000D418
        public static T InvokeMethod<T>(this object source, string methodName, params object[] parameters)
        {
            if (source == null || methodName == null)
            {
                throw new ArgumentNullException((source == null) ? "source" : "methodName");
            }
            Type type = source.GetType();
            MethodInfo method = type.GetMethod(methodName);
            if (method == null)
            {
                throw new ArgumentException($"未能找到方法“{methodName}”。");
            }
            object obj = method.Invoke(source, parameters);
            if (!(obj is T))
            {
                return default(T);
            }
            return (T)((object)obj);
        }

        /// <summary>
        /// 获取对象实例中指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性值的类型。</typeparam>
        /// <param name="source">包含 <paramref name="propertyName" /> 属性的对象实例。</param>
        /// <param name="propertyName">属性的名称。</param>
        /// <returns>具有指定返回指定类型的属性的值。如果对象中不包含指定的属性则返回 <typeparamref name="T" /> 的默认值。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="propertyName" /> 为 <c>null</c>。</exception>
        /// <history>
        ///     2011-02-15 17:28:33 郑建(Roc Zheng) 修改方法实现
        /// </history>
        // Token: 0x06000421 RID: 1057 RVA: 0x0000F29C File Offset: 0x0000D49C
        public static T GetPropertyValue<T>(this object source, string propertyName)
        {
            return source.GetPropertyValue(propertyName, default(T), false);
        }

        /// <summary>
        /// 获取对象实例中指定属性的值。
        /// </summary>
        /// <typeparam name="T">属性值的类型。</typeparam>
        /// <param name="source">包含 <paramref name="propertyName" /> 属性的对象实例。</param>
        /// <param name="propertyName">属性的名称。</param>
        /// <param name="defaultValue">默认值，如果对象中不包含指定的属性且 <paramref name="ignoreErrors" /> 为 ture 时的返回值。</param>
        /// <param name="ignoreErrors">是否忽略属性未找到的错误。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的对象，包含被调用属性的返回值，如果属性返回值的类型不是 <typeparamref name="T" /> 则返回 <typeparamref name="T" /> 的类型默认值。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="propertyName" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">未找到 <paramref name="propertyName" /> 指定的属性。</exception>
        /// <history>
        ///     2011-02-15 17:21:33 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000422 RID: 1058 RVA: 0x0000F2BC File Offset: 0x0000D4BC
        public static T GetPropertyValue<T>(this object source, string propertyName, T defaultValue, bool ignoreErrors = false)
        {
            if (source == null || propertyName == null)
            {
                throw new ArgumentNullException((source == null) ? "source" : "propertyName");
            }
            object propertyValue = source.GetPropertyValue(propertyName, defaultValue, ignoreErrors);
            if (!(propertyValue is T))
            {
                return defaultValue;
            }
            return (T)((object)propertyValue);
        }

        /// <summary>
        /// 获取对象实例中指定属性的值。
        /// </summary>
        /// <param name="source">包含 <paramref name="propertyName" /> 属性的对象实例。</param>
        /// <param name="propertyName">属性的名称。</param>
        /// <returns><see cref="T:System.Object" />，表示对象实例中指定属性的值。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="propertyName" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.Reflection.AmbiguousMatchException">找到不止一个具有指定名称的属性。</exception>
        /// <history>
        ///     2011-02-15 17:24:45 郑建(Roc Zheng) 修改方法实现
        /// </history>
        // Token: 0x06000423 RID: 1059 RVA: 0x0000F303 File Offset: 0x0000D503
        public static object GetPropertyValue(this object source, string propertyName)
        {
            return source.GetPropertyValue(propertyName, null, false);
        }

        /// <summary>
        /// 获取对象实例中指定属性的值。
        /// </summary>
        /// <param name="source">包含 <paramref name="propertyName" /> 属性的对象实例。</param>
        /// <param name="propertyName">属性的名称。</param>
        /// <param name="defaultValue">默认值，如果对象中不包含指定的属性且 <paramref name="ignoreErrors" /> 为 ture 时的返回值。</param>
        /// <param name="ignoreErrors">是否忽略属性未找到的错误。</param>
        /// <returns><see cref="T:System.Object" />，表示对象实例中指定属性的值。如果对象中不包含指定的属性且 <paramref name="ignoreErrors" /> 为 ture 时则返回 <paramref name="defaultValue" />。</returns>
        public static object GetPropertyValue(this object source, string propertyName, object defaultValue, bool ignoreErrors = false)
        {
            if (source == null || propertyName == null)
            {
                throw new ArgumentNullException((source == null) ? "source" : "propertyName");
            }
            Type type = source.GetType();
            PropertyInfo property = type.GetProperty(propertyName, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (null == property && !ignoreErrors)
            {
                throw new ArgumentException($"未找到属性“{propertyName}”。");
            }
            object result;
            try
            {
                result = property.GetValue(source, null);
            }
            catch
            {
                if (!ignoreErrors)
                {
                    throw;
                }
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        /// 设置对象实例中指定属性的值。
        /// </summary>
        /// <param name="source">包含 <paramref name="propertyName" /> 属性的对象实例。</param>
        /// <param name="propertyName">属性的名称。</param>
        /// <param name="value">用于设置属性的新值。</param>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 或 <paramref name="propertyName" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">未找到 <paramref name="propertyName" /> 指定的属性或指定的属性是只读的。</exception>
        /// <history>
        ///     2011-02-15 22:18:58 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000425 RID: 1061 RVA: 0x0000F39C File Offset: 0x0000D59C
        public static void SetPropertyValue(this object source, string propertyName, object value)
        {
            if (source == null || propertyName == null)
            {
                throw new ArgumentNullException((source == null) ? "source" : "propertyName");
            }
            Type type = source.GetType();
            PropertyInfo property = type.GetProperty(propertyName);
            if (property == null)
            {
                throw new ArgumentException($"未找到属性“{propertyName}”。");
            }
            if (!property.CanWrite)
            {
                throw new ArgumentException($"无法对属性“{propertyName}”赋值，它是只读的。");
            }
            property.SetValue(source, value, null);
        }

        /// <summary>
        /// 获取对象实例类型中定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">对象实例或类型。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <history>
        ///     2011-02-16 10:07:09 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000426 RID: 1062 RVA: 0x0000F42F File Offset: 0x0000D62F
        public static T GetAttribute<T>(this object source) where T : Attribute
        {
            return source.GetAttribute<T>(true);
        }

        /// <summary>
        /// 获取对象实例类型中定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">对象实例或类型。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <history>
        ///     2011-02-16 10:05:32 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000427 RID: 1063 RVA: 0x0000F438 File Offset: 0x0000D638
        public static T GetAttribute<T>(this object source, bool includeInherited) where T : Attribute
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            object[] customAttributes;
            if (source is MemberInfo)
            {
                MemberInfo memberInfo = source as MemberInfo;
                customAttributes = memberInfo.GetCustomAttributes(typeof(T), includeInherited);
            }
            else
            {
                Type type = source.GetType();
                customAttributes = type.GetCustomAttributes(typeof(T), includeInherited);
            }
            if (customAttributes.Length > 0)
            {
                return customAttributes[0] as T;
            }
            return default(T);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 21:45:23 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000428 RID: 1064 RVA: 0x0000F4AE File Offset: 0x0000D6AE
        public static T GetMemberAttribute<T>(this object source, string memberName) where T : Attribute
        {
            return source.GetMemberAttribute<T>(memberName, MemberTypes.All, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, true);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，由一个或多个指定搜索执行方式的 <see cref="T:System.Reflection.BindingFlags" /> 组成。 - 或 - 零，返回空数组。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 21:45:08 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000429 RID: 1065 RVA: 0x0000F4BF File Offset: 0x0000D6BF
        public static T GetMemberAttribute<T>(this object source, string memberName, BindingFlags bindingAttr) where T : Attribute
        {
            return source.GetMemberAttribute<T>(memberName, MemberTypes.All, bindingAttr, true);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 21:44:55 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600042A RID: 1066 RVA: 0x0000F4CF File Offset: 0x0000D6CF
        public static T GetMemberAttribute<T>(this object source, string memberName, bool includeInherited) where T : Attribute
        {
            return source.GetMemberAttribute<T>(memberName, MemberTypes.All, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, includeInherited);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，由一个或多个指定搜索执行方式的 <see cref="T:System.Reflection.BindingFlags" /> 组成。 - 或 - 零，返回空数组。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 21:44:40 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600042B RID: 1067 RVA: 0x0000F4E0 File Offset: 0x0000D6E0
        public static T GetMemberAttribute<T>(this object source, string memberName, BindingFlags bindingAttr, bool includeInherited) where T : Attribute
        {
            return source.GetMemberAttribute<T>(memberName, MemberTypes.All, bindingAttr, includeInherited);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="memberType">要搜索的 <see cref="T:System.Reflection.MemberTypes" /> 值。</param>
        /// <param name="bindingAttr">一个位屏蔽，由一个或多个指定搜索执行方式的 <see cref="T:System.Reflection.BindingFlags" /> 组成。 - 或 - 零，返回空数组。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个 <typeparamref name="T" /> 类型的自定义属性的实例，如果未找到则返回 <c>null</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 21:44:21 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600042C RID: 1068 RVA: 0x0000F4F0 File Offset: 0x0000D6F0
        public static T GetMemberAttribute<T>(this object source, string memberName, MemberTypes memberType, BindingFlags bindingAttr, bool includeInherited) where T : Attribute
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(memberName))
            {
                throw new ArgumentException("成员的名称不能为空或空字符串。");
            }
            Type type = (source as Type) ?? source.GetType();
            MemberInfo[] member = type.GetMember(memberName, memberType, bindingAttr);
            if (member == null || member.Length < 1 || member[0] == null)
            {
                throw new ArgumentException($"类型“{type.ToString()}”中不存在名称为“{memberName}”的成员信息。");
            }
            MemberInfo memberInfo = member[0];
            object[] customAttributes = memberInfo.GetCustomAttributes(typeof(T), includeInherited);
            if (customAttributes.Length > 0)
            {
                return customAttributes[0] as T;
            }
            return default(T);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 22:46:06 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600042D RID: 1069 RVA: 0x0000F5BA File Offset: 0x0000D7BA
        public static IEnumerable<T> GetMemberAttributes<T>(this object source, string memberName) where T : Attribute
        {
            return source.GetMemberAttributes<T>(memberName, MemberTypes.All, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, true);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，由一个或多个指定搜索执行方式的 <see cref="T:System.Reflection.BindingFlags" /> 组成。 - 或 - 零，返回空数组。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 22:47:14 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600042E RID: 1070 RVA: 0x0000F5CB File Offset: 0x0000D7CB
        public static IEnumerable<T> GetMemberAttributes<T>(this object source, string memberName, BindingFlags bindingAttr) where T : Attribute
        {
            return source.GetMemberAttributes<T>(memberName, MemberTypes.All, bindingAttr, true);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 22:43:51 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600042F RID: 1071 RVA: 0x0000F5DB File Offset: 0x0000D7DB
        public static IEnumerable<T> GetMemberAttributes<T>(this object source, string memberName, bool includeInherited) where T : Attribute
        {
            return source.GetMemberAttributes<T>(memberName, MemberTypes.All, BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, includeInherited);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="bindingAttr">一个位屏蔽，由一个或多个指定搜索执行方式的 <see cref="T:System.Reflection.BindingFlags" /> 组成。 - 或 - 零，返回空数组。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 22:42:35 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000430 RID: 1072 RVA: 0x0000F5EC File Offset: 0x0000D7EC
        public static IEnumerable<T> GetMemberAttributes<T>(this object source, string memberName, BindingFlags bindingAttr, bool includeInherited) where T : Attribute
        {
            return source.GetMemberAttributes<T>(memberName, MemberTypes.All, bindingAttr, includeInherited);
        }

        /// <summary>
        /// 获取对象实例类型中指定成员上定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">包含名称为 <paramref name="memberName" /> 的类型或类型实例。</param>
        /// <param name="memberName">成员的名称。</param>
        /// <param name="memberType">要搜索的 <see cref="T:System.Reflection.MemberTypes" /> 值。</param>
        /// <param name="bindingAttr">一个位屏蔽，由一个或多个指定搜索执行方式的 <see cref="T:System.Reflection.BindingFlags" /> 组成。 - 或 - 零，返回空数组。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <exception cref="T:System.ArgumentException">成员的名称不能为空或空字符串。-或- <paramref name="source" /> 类型中不存在名称为 <paramref name="memberName" /> 的成员信息。</exception>
        /// <history>
        /// 2011-05-26 22:18:56 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000431 RID: 1073 RVA: 0x0000F5FC File Offset: 0x0000D7FC
        public static IEnumerable<T> GetMemberAttributes<T>(this object source, string memberName, MemberTypes memberType, BindingFlags bindingAttr, bool includeInherited) where T : Attribute
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(memberName))
            {
                throw new ArgumentException("成员的名称不能为空或空字符串。");
            }
            Type type = (source as Type) ?? source.GetType();
            MemberInfo[] member = type.GetMember(memberName, memberType, bindingAttr);
            if (member == null || member.Length < 1 || member[0] == null)
            {
                throw new ArgumentException($"类型“{type.ToString()}”中不存在名称为“{member}”的成员信息。");
            }
            MemberInfo memberInfo = member[0];
            return memberInfo.GetCustomAttributes(typeof(T), includeInherited).OfType<T>();
        }

        /// <summary>
        /// 获取对象实例类型中定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">对象实例或类型。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <history>
        ///     2011-02-16 10:15:07 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000432 RID: 1074 RVA: 0x0000F6A8 File Offset: 0x0000D8A8
        public static IEnumerable<T> GetAttributes<T>(this object source) where T : Attribute
        {
            return source.GetAttributes<T>(true);
        }

        /// <summary>
        /// 获取对象实例类型中定义的 <typeparamref name="T" /> 类型的自定义属性序列。
        /// </summary>
        /// <typeparam name="T">自定义属性的类型，必须是 <see cref="T:System.Attribute" /> 类的子类。</typeparam>
        /// <param name="source">对象实例或类型。</param>
        /// <param name="includeInherited">指定是否搜索该成员的继承链以查找这些属性。</param>
        /// <returns>一个可枚举的 <typeparamref name="T" /> 类型的自定义属性序列。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        /// <history>
        ///     2011-02-16 10:13:44 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000433 RID: 1075 RVA: 0x0000F6B1 File Offset: 0x0000D8B1
        public static IEnumerable<T> GetAttributes<T>(this object source, bool includeInherited) where T : Attribute
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return ((source as Type) ?? source.GetType()).GetCustomAttributes(typeof(T), includeInherited).OfType<T>();
        }

        /// <summary>
        /// 判断给定值是否在 <paramref name="values" /> 序列中存在。
        /// </summary>
        /// <typeparam name="T">实例的类型。</typeparam>
        /// <param name="source">实例值。</param>
        /// <param name="values">值序列。</param>
        /// <returns>如果 <paramref name="source" /> 存在于在 <paramref name="values" /> 中则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        /// <exception cref="T:System.ArgumentNullException"><paramref name="source" /> 为 <c>null</c>。</exception>
        // Token: 0x06000434 RID: 1076 RVA: 0x0000F6E6 File Offset: 0x0000D8E6
        public static bool In<T>(this T source, params T[] values)
        {
            return source.In((IEnumerable<T>)values);
        }

        /// <summary>
        /// 判断给定值是否在 <paramref name="values" /> 序列中存在。
        /// </summary>
        /// <typeparam name="T">实例的类型。</typeparam>
        /// <param name="source">实例值。</param>
        /// <param name="values">值序列。</param>
        /// <returns>如果 <paramref name="source" /> 存在于在 <paramref name="values" /> 中则返回 <c>true</c>，否则返回 <c>false</c>。</returns>
        /// <history>
        ///     2011-02-15 14:56:50 郑建(Roc Zheng) 修改方法实现
        /// </history>
        // Token: 0x06000435 RID: 1077 RVA: 0x0000F6F4 File Offset: 0x0000D8F4
        public static bool In<T>(this T source, IEnumerable<T> values)
        {
            if (source == null && values == null)
            {
                return true;
            }
            if (values == null)
            {
                return false;
            }
            if (values is T[])
            {
                return Array.IndexOf<T>(values as T[], source) != -1;
            }
            foreach (T t in values)
            {
                if (source == null && t == null)
                {
                    return true;
                }
                if (source is IEquatable<T>)
                {
                    IEquatable<T> equatable = source as IEquatable<T>;
                    if (equatable.Equals(t))
                    {
                        return true;
                    }
                }
                else if (source is IComparable<T>)
                {
                    IComparable<T> comparable = source as IComparable<T>;
                    if (comparable.CompareTo(t) == 0)
                    {
                        return true;
                    }
                }
                else if (source is IComparable)
                {
                    IComparable comparable2 = source as IComparable;
                    if (comparable2.CompareTo(t) == 0)
                    {
                        return true;
                    }
                }
                else if (source != null && source.Equals(t))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断给定值是否在 <paramref name="values" /> 序列中不存在。
        /// </summary>
        /// <typeparam name="T">实例的类型。</typeparam>
        /// <param name="source">实例值。</param>
        /// <param name="values">值序列。</param>
        /// <returns>如果不存在则返回 <c>true</c>，反之为 <c>false</c>。</returns>
        /// <history>
        ///     2011-03-18 21:03:41 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000436 RID: 1078 RVA: 0x0000F828 File Offset: 0x0000DA28
        public static bool NotIn<T>(this T source, params T[] values)
        {
            return !source.In(values);
        }

        /// <summary>
        /// 判断给定值是否在 <paramref name="values" /> 序列中不存在。
        /// </summary>
        /// <typeparam name="T">实例的类型。</typeparam>
        /// <param name="source">实例值。</param>
        /// <param name="values">值序列。</param>
        /// <returns>如果不存在则返回 <c>true</c>，反之为 <c>false</c>。</returns>
        /// <history>
        ///     2011-03-18 21:04:47 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000437 RID: 1079 RVA: 0x0000F834 File Offset: 0x0000DA34
        public static bool NotIn<T>(this T source, IEnumerable<T> values)
        {
            return !source.In(values);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        /// <history>
        /// 2011-03-18 21:18:30 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000438 RID: 1080 RVA: 0x0000F840 File Offset: 0x0000DA40
        public static T ConvertTo<T>(this object source)
        {
            return source.ConvertTo(default(T));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <param name="ignoreException"></param>
        /// <returns></returns>
        /// <history>
        /// 2011-03-18 21:20:46 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x06000439 RID: 1081 RVA: 0x0000F85C File Offset: 0x0000DA5C
        public static T ConvertTo<T>(this object source, T defaultValue, bool ignoreException)
        {
            if (ignoreException)
            {
                try
                {
                    return source.ConvertTo<T>();
                }
                catch
                {
                    return defaultValue;
                }
            }
            return source.ConvertTo<T>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        /// <history>
        /// 2011-03-18 21:17:42 郑建(Roc Zheng) 创建
        /// </history>
        // Token: 0x0600043A RID: 1082 RVA: 0x0000F894 File Offset: 0x0000DA94
        public static T ConvertTo<T>(this object source, T defaultValue)
        {
            if (source == null)
            {
                return defaultValue;
            }
            Type typeFromHandle = typeof(T);
            Type type = source.GetType();
            if (type == typeFromHandle || type.IsSubclassOf(typeFromHandle))
            {
                return (T)((object)source);
            }
            if (source is IConvertible)
            {
                return (T)((object)(source as IConvertible).ToType(typeof(T), null));
            }
            TypeConverter converter = TypeDescriptor.GetConverter(source);
            if (converter != null && converter.CanConvertTo(typeFromHandle))
            {
                return (T)((object)converter.ConvertTo(source, typeFromHandle));
            }
            converter = TypeDescriptor.GetConverter(typeFromHandle);
            if (converter != null && converter.CanConvertFrom(type))
            {
                return (T)((object)converter.ConvertFrom(type));
            }
            return (T)((object)Convert.ChangeType(source, typeof(T)));
        }


        /// <summary>
        /// 将当前对象强制转换为指定的类型。
        /// </summary>
        /// <typeparam name="T"><paramref name="source" /> 要转换成的类型。</typeparam>
        /// <param name="source">要转换的对象。</param>
        /// <returns>已转换为指定类型的对象。</returns>
        /// <remarks>与 <code>(T)source</code> 等价。</remarks>
        // Token: 0x06000443 RID: 1091 RVA: 0x0000F9E3 File Offset: 0x0000DBE3
        public static T CastAs<T>(this object source)
        {
            return (T)((object)source);
        }

        /// <summary>
        /// 将当前对象强制转换为指定的类型。
        /// </summary>
        /// <typeparam name="T"><paramref name="source" /> 要转换成的类型。</typeparam>
        /// <param name="source">要转换的对象。</param>
        /// <param name="defaultValue">指定转换失败时的默认值。</param>
        /// <returns>已转换为指定类型的对象。</returns>
        /// <remarks>等价于：
        ///     <code>
        ///     if(source is T)
        ///         return (T)source;
        ///
        ///     return defaultValue;
        ///     </code>
        /// </remarks>
        // Token: 0x06000444 RID: 1092 RVA: 0x0000F9EB File Offset: 0x0000DBEB
        public static T CastAs<T>(this object source, T defaultValue)
        {
            if (source is T)
            {
                return (T)((object)source);
            }
            return defaultValue;
        }

        /// <summary>
        /// 对当前对象执行深拷贝。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="source">要执行深拷贝的对象。</param>
        /// <returns>返回拷贝后的对象。</returns>
        /// <exception cref="T:System.ArgumentException"><typeparamref name="T" /> 不是可串行的类型。</exception>
        // Token: 0x06000445 RID: 1093 RVA: 0x0000FA00 File Offset: 0x0000DC00
        public static T Clone<T>(this T source)
        {
            if (!typeof(T).IsSerializable)
            {
                throw new ArgumentException("source not serializable");
            }
            if (object.ReferenceEquals(source, null))
            {
                return default(T);
            }
            IFormatter formatter = new BinaryFormatter();
            T result;
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, source);
                stream.Seek(0L, SeekOrigin.Begin);
                result = (T)((object)formatter.Deserialize(stream));
            }
            return result;
        }

        /// <summary>
        /// 检测当前对象是否为空引用。
        /// </summary>
        /// <typeparam name="T">对象的类型。</typeparam>
        /// <param name="source">要检测的对象。</param>
        /// <returns>如果当前对象是空引用则返回 true，否则返回 false。</returns>
        // Token: 0x06000446 RID: 1094 RVA: 0x0000FA98 File Offset: 0x0000DC98
        public static bool IsNull<T>(this T source)
        {
            return object.ReferenceEquals(source, null);
        }

        // Token: 0x06000447 RID: 1095 RVA: 0x0000FAA8 File Offset: 0x0000DCA8
        private static object InvokeFormaterMethod(object instance, string methodExpression, params object[] args)
        {
            if (instance == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(methodExpression))
            {
                throw new ArgumentNullException("methodExpression");
            }
            int num = methodExpression.IndexOf('(') + 1;
            int num2 = methodExpression.IndexOf(')', num);
            string text = methodExpression.Substring(num, num2 - num);
            string[] array = text.Split(new char[]
            {
                ','
            }, StringSplitOptions.RemoveEmptyEntries);
            object[] array2 = null;
            Type[] array3 = new Type[array.Length];
            if (array != null && 0 < array.Length)
            {
                array2 = new object[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    string text2 = array[i];
                    if (text2.StartsWith("@"))
                    {
                        int num3 = int.Parse(text2.Substring(1));
                        array2[i] = args[num3];
                        array3[i] = args[num3].GetType();
                    }
                    else
                    {
                        array2[i] = text2;
                        array3[i] = text2.GetType();
                    }
                }
            }
            string name = methodExpression.Substring(0, num - 1);
            MethodInfo method = instance.GetType().GetMethod(name, BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, array3, null);
            return method.Invoke(instance, array2);
        }

        // Token: 0x06000448 RID: 1096 RVA: 0x0000FBB4 File Offset: 0x0000DDB4
        private static object InvokeFormaterIndexer(object instance, string indexerExpression, params object[] args)
        {
            if (instance == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(indexerExpression))
            {
                throw new ArgumentNullException("indexerExpression");
            }
            int num = indexerExpression.IndexOf('[') + 1;
            int num2 = indexerExpression.IndexOf(']', num);
            string text = indexerExpression.Substring(num, num2 - num);
            if (text.Length < 1)
            {
                throw new ArgumentException("需要提供索引值。", indexerExpression);
            }
            object[] index = new object[]
            {
                int.Parse(text)
            };
            Type[] types = new Type[]
            {
                typeof(int)
            };
            indexerExpression.Substring(0, num - 1);
            Type type = instance.GetType();
            PropertyInfo property = type.GetProperty("Item", BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public, null, type, types, null);
            return property.GetValue(instance, index);
        }

        // Token: 0x06000449 RID: 1097 RVA: 0x0000FC74 File Offset: 0x0000DE74
        private static object InvokeFormatterArrayIndexer(object instance, string arrayIndexerExpression, params object[] args)
        {
            if (instance == null)
            {
                return null;
            }
            if (string.IsNullOrWhiteSpace(arrayIndexerExpression))
            {
                throw new ArgumentNullException("indexerExpression");
            }
            int num = arrayIndexerExpression.IndexOf('[') + 1;
            int num2 = arrayIndexerExpression.IndexOf(']', num);
            string text = arrayIndexerExpression.Substring(num, num2 - num);
            if (text.Length < 1)
            {
                throw new ArgumentException("需要提供索引值。", arrayIndexerExpression);
            }
            object[] array = new object[]
            {
                int.Parse(text)
            };
            string propertyName = arrayIndexerExpression.Substring(0, num - 1);
            Array array2 = (Array)instance.GetPropertyValue(propertyName);
            return array2.GetValue((int)array[0]);
        }



        // Token: 0x06000251 RID: 593 RVA: 0x00008698 File Offset: 0x00006898
        public static void TraversalMethodsInfo(this object source, Func<MethodInfo, bool> method)
        {
            source.GetType().TraversalMethodsInfo(method);
        }

        // Token: 0x06000252 RID: 594 RVA: 0x000086A8 File Offset: 0x000068A8
        public static void TraversalFieldsInfo(this object source, Func<FieldInfo, object, bool> method)
        {
            if (method == null || source == null)
            {
                return;
            }
            FieldInfo[] fields = source.GetType().GetFields();
            foreach (FieldInfo fieldInfo in fields)
            {
                object value = fieldInfo.GetValue(source);
                if (!method(fieldInfo, value))
                {
                    break;
                }
            }
        }

        // Token: 0x06000253 RID: 595 RVA: 0x000086F8 File Offset: 0x000068F8
        public static void TraversalPropertiesInfo(this object source, Func<string, object, bool> method)
        {
            if (method == null || source == null)
            {
                return;
            }
            PropertyInfo[] properties = source.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanRead)
                {
                    object value = propertyInfo.GetValue(source, null);
                    if (!method(propertyInfo.Name, value))
                    {
                        break;
                    }
                }
            }
        }

        // Token: 0x06000254 RID: 596 RVA: 0x0000875C File Offset: 0x0000695C
        public static void TraversalPropertiesInfo(this object source, Func<PropertyInfo, object, bool> method)
        {
            if (method == null || source == null)
            {
                return;
            }
            PropertyInfo[] array = (from c in source.GetType().GetProperties()
                                    orderby c.MetadataToken
                                    select c).ToArray<PropertyInfo>();
            foreach (PropertyInfo propertyInfo in array)
            {
                if (propertyInfo.CanRead)
                {
                    object value = propertyInfo.GetValue(source, null);
                    if (!method(propertyInfo, value))
                    {
                        break;
                    }
                }
            }
        }

        // Token: 0x06000255 RID: 597 RVA: 0x000087DC File Offset: 0x000069DC
        public static void TraversalPropertiesInfo(this object source, Func<PropertyInfo, object, object, bool> method, object argument)
        {
            if (method == null || source == null)
            {
                return;
            }
            PropertyInfo[] properties = source.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                if (propertyInfo.CanRead)
                {
                    object value = propertyInfo.GetValue(source, null);
                    if (!method(propertyInfo, value, argument))
                    {
                        break;
                    }
                }
            }
        }
    }
}

