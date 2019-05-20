using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    /// <summary>
    /// 表示代码构成的动态查询信息类。
    /// </summary>
    [Serializable]
    public class CodeDynamicQuery
    {
        /// <summary>
        /// 查询匹配代码。
        /// </summary>
        public string PredicateCode { get; private set; }

        /// <summary>
        /// 参数数组。
        /// </summary>
        public object[] Arguments { get; private set; }

        /// <summary>
        /// 初始化 <see cref="CodeDynamicQuery"/> 类的新实例。
        /// </summary>
        /// <param name="predicateCode">查询匹配代码。</param>
        /// <param name="arguments"><paramref name="predicateCode"/> 中参数的对应值的数组</param>
        public CodeDynamicQuery(string predicateCode, object[] arguments)
        {
            if (null == predicateCode)
                throw new ArgumentNullException("predicateCode");

            if (String.IsNullOrWhiteSpace(predicateCode))
                throw new ArgumentException("查询匹配代码不能为空或仅由空白字符组成的字符串。", "predicateCode");

            this.PredicateCode = predicateCode;
            this.Arguments = arguments;
        }
    }
}
