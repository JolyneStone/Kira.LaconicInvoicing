using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Kira.LaconicInvoicing
{
    // Token: 0x02000076 RID: 118
    public static class DynamicExpression
    {
        // Token: 0x06000313 RID: 787 RVA: 0x0000B650 File Offset: 0x00009850
        public static Expression Parse(Type resultType, string expression, params object[] values)
        {
            ExpressionParser expressionParser = new ExpressionParser(null, expression, values, false);
            return expressionParser.Parse(resultType);
        }

        // Token: 0x06000314 RID: 788 RVA: 0x0000B670 File Offset: 0x00009870
        public static LambdaExpression ParseLambda(Type itType, Type resultType, string expression, params object[] values)
        {
            return DynamicExpression.ParseLambda(new ParameterExpression[]
            {
                Expression.Parameter(itType, "")
            }, resultType, expression, false, values);
        }

        // Token: 0x06000315 RID: 789 RVA: 0x0000B69C File Offset: 0x0000989C
        public static LambdaExpression ParseLambda(Type itType, Type resultType, string parameterName, string expression, params object[] values)
        {
            if (string.IsNullOrWhiteSpace(parameterName))
            {
                return DynamicExpression.ParseLambda(itType, resultType, expression, values);
            }
            return DynamicExpression.ParseLambda(new ParameterExpression[]
            {
                Expression.Parameter(itType, parameterName)
            }, resultType, expression, true, values);
        }

        // Token: 0x06000316 RID: 790 RVA: 0x0000B6D8 File Offset: 0x000098D8
        public static LambdaExpression ParseLambda(ParameterExpression[] parameters, Type resultType, string expression, params object[] values)
        {
            return DynamicExpression.ParseLambda(parameters, resultType, expression, false, values);
        }

        // Token: 0x06000317 RID: 791 RVA: 0x0000B6E4 File Offset: 0x000098E4
        public static LambdaExpression ParseLambda(ParameterExpression[] parameters, Type resultType, string expression, bool namedParameter, params object[] values)
        {
            ExpressionParser expressionParser = new ExpressionParser(parameters, expression, values, namedParameter);
            return Expression.Lambda(expressionParser.Parse(resultType), parameters);
        }

        // Token: 0x06000318 RID: 792 RVA: 0x0000B709 File Offset: 0x00009909
        public static Expression<Func<T, S>> ParseLambda<T, S>(string expression, params object[] values)
        {
            return (Expression<Func<T, S>>)DynamicExpression.ParseLambda(typeof(T), typeof(S), expression, values);
        }

        // Token: 0x06000319 RID: 793 RVA: 0x0000B72B File Offset: 0x0000992B
        public static Type CreateClass(params DynamicProperty[] properties)
        {
            return ClassFactory.Instance.GetDynamicClass(properties);
        }

        // Token: 0x0600031A RID: 794 RVA: 0x0000B738 File Offset: 0x00009938
        public static Type CreateClass(IEnumerable<DynamicProperty> properties)
        {
            return ClassFactory.Instance.GetDynamicClass(properties);
        }
    }
}
