using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Spatial;

namespace Kira.LaconicInvoicing
{
    internal class ExpressionParser
    {
        public ExpressionParser(ParameterExpression[] parameters, string expression, object[] values, bool namedParameter = false)
        {
            if (expression == null)
            {
                throw new ArgumentNullException("expression");
            }
            if (ExpressionParser.keywords == null)
            {
                ExpressionParser.keywords = ExpressionParser.CreateKeywords();
            }
            this.symbols = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.literals = new Dictionary<Expression, string>();
            if (parameters != null)
            {
                this.ProcessParameters(parameters, namedParameter);
            }
            if (values != null)
            {
                this.ProcessValues(values);
            }
            this.text = expression;
            this.textLen = this.text.Length;
            this.SetTextPos(0);
            this.NextToken();
        }

        private void ProcessParameters(ParameterExpression[] parameters, bool namedParameter = false)
        {
            if (namedParameter)
            {
                this.it = parameters[0];
                return;
            }
            foreach (ParameterExpression parameterExpression in parameters)
            {
                if (!string.IsNullOrEmpty(parameterExpression.Name))
                {
                    this.AddSymbol(parameterExpression.Name, parameterExpression);
                }
            }
            if (parameters.Length != 1)
            {
                return;
            }
            ParameterExpression parameterExpression2 = parameters[0];
            if (parameterExpression2.Name.IsNullOrEmpty())
            {
                this.it = parameterExpression2;
            }
        }

        private void ProcessValues(object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                object obj = values[i];
                if (i == values.Length - 1 && obj is IDictionary<string, object>)
                {
                    this.externals = (IDictionary<string, object>)obj;
                }
                else
                {
                    this.AddSymbol("@" + i.ToString(CultureInfo.InvariantCulture), obj);
                }
            }
        }

        private void AddSymbol(string name, object value)
        {
            if (this.symbols.ContainsKey(name))
            {
                throw this.ParseError("标识符“{0}”被定义了多次", new object[]
                {
                    name
                });
            }
            this.symbols.Add(name, value);
        }

        public Expression Parse(Type resultType)
        {
            int pos = this.token.pos;
            Expression expression = this.ParseExpression();
            if (resultType != null && (expression = this.PromoteExpression(expression, resultType, true)) == null)
            {
                throw this.ParseError(pos, "Expression of type '{0}' expected", new object[]
                {
                    ExpressionParser.GetTypeName(resultType)
                });
            }
            this.ValidateToken(ExpressionParser.TokenId.End, "语法错误");
            return expression;
        }

        public IEnumerable<DynamicOrdering> ParseOrdering()
        {
            List<DynamicOrdering> list = new List<DynamicOrdering>();
            for (; ; )
            {
                Expression selector = this.ParseExpression();
                bool ascending = true;
                if (this.TokenIdentifierIs("asc") || this.TokenIdentifierIs("ascending"))
                {
                    this.NextToken();
                }
                else if (this.TokenIdentifierIs("desc") || this.TokenIdentifierIs("descending"))
                {
                    this.NextToken();
                    ascending = false;
                }
                list.Add(new DynamicOrdering
                {
                    Selector = selector,
                    Ascending = ascending
                });
                if (this.token.id != ExpressionParser.TokenId.Comma)
                {
                    break;
                }
                this.NextToken();
            }
            this.ValidateToken(ExpressionParser.TokenId.End, "语法错误");
            return list;
        }

        private Expression ParseExpression()
        {
            int pos = this.token.pos;
            Expression expression = this.ParseLogicalOr();
            if (this.token.id == ExpressionParser.TokenId.Question)
            {
                this.NextToken();
                Expression expr = this.ParseExpression();
                this.ValidateToken(ExpressionParser.TokenId.Colon, "缺少“:”");
                this.NextToken();
                Expression expr2 = this.ParseExpression();
                expression = this.GenerateConditional(expression, expr, expr2, pos);
            }
            return expression;
        }

        private Expression ParseLogicalOr()
        {
            Expression expression = this.ParseLogicalAnd();
            while (this.token.id == ExpressionParser.TokenId.DoubleBar || this.TokenIdentifierIs("or"))
            {
                ExpressionParser.Token token = this.token;
                this.NextToken();
                Expression right = this.ParseLogicalAnd();
                this.CheckAndPromoteOperands(typeof(ExpressionParser.ILogicalSignatures), token.text, ref expression, ref right, token.pos);
                expression = Expression.OrElse(expression, right);
            }
            return expression;
        }

        private Expression ParseLogicalAnd()
        {
            Expression expression = this.ParseComparison();
            while (this.token.id == ExpressionParser.TokenId.DoubleAmphersand || this.TokenIdentifierIs("and"))
            {
                ExpressionParser.Token token = this.token;
                this.NextToken();
                Expression right = this.ParseComparison();
                this.CheckAndPromoteOperands(typeof(ExpressionParser.ILogicalSignatures), token.text, ref expression, ref right, token.pos);
                expression = Expression.AndAlso(expression, right);
            }
            return expression;
        }

        private Expression ParseComparison()
        {
            Expression expression = this.ParseAdditive();
            while (this.token.id == ExpressionParser.TokenId.Equal || this.token.id == ExpressionParser.TokenId.DoubleEqual || this.token.id == ExpressionParser.TokenId.ExclamationEqual || this.token.id == ExpressionParser.TokenId.LessGreater || this.token.id == ExpressionParser.TokenId.GreaterThan || this.token.id == ExpressionParser.TokenId.GreaterThanEqual || this.token.id == ExpressionParser.TokenId.LessThan || this.token.id == ExpressionParser.TokenId.LessThanEqual)
            {
                ExpressionParser.Token token = this.token;
                this.NextToken();
                Expression expression2 = this.ParseAdditive();
                bool flag = token.id == ExpressionParser.TokenId.Equal || token.id == ExpressionParser.TokenId.DoubleEqual || token.id == ExpressionParser.TokenId.ExclamationEqual || token.id == ExpressionParser.TokenId.LessGreater;
                if (flag && !expression.Type.IsValueType && !expression2.Type.IsValueType)
                {
                    if (expression.Type != expression2.Type)
                    {
                        if (expression.Type.IsAssignableFrom(expression2.Type))
                        {
                            expression2 = Expression.Convert(expression2, expression.Type);
                        }
                        else
                        {
                            if (!expression2.Type.IsAssignableFrom(expression.Type))
                            {
                                throw this.IncompatibleOperandsError(token.text, expression, expression2, token.pos);
                            }
                            expression = Expression.Convert(expression, expression2.Type);
                        }
                    }
                }
                else if (ExpressionParser.IsEnumType(expression.Type) || ExpressionParser.IsEnumType(expression2.Type))
                {
                    if (expression.Type != expression2.Type)
                    {
                        Expression expression3;
                        if ((expression3 = this.PromoteExpression(expression2, expression.Type, true)) != null)
                        {
                            expression2 = expression3;
                        }
                        else
                        {
                            if ((expression3 = this.PromoteExpression(expression, expression2.Type, true)) == null)
                            {
                                throw this.IncompatibleOperandsError(token.text, expression, expression2, token.pos);
                            }
                            expression = expression3;
                        }
                    }
                }
                else
                {
                    this.CheckAndPromoteOperands(flag ? typeof(ExpressionParser.IEqualitySignatures) : typeof(ExpressionParser.IRelationalSignatures), token.text, ref expression, ref expression2, token.pos);
                }
                switch (token.id)
                {
                    case ExpressionParser.TokenId.LessThan:
                        expression = this.GenerateLessThan(expression, expression2);
                        break;
                    case ExpressionParser.TokenId.Equal:
                    case ExpressionParser.TokenId.DoubleEqual:
                        expression = this.GenerateEqual(expression, expression2);
                        break;
                    case ExpressionParser.TokenId.GreaterThan:
                        expression = this.GenerateGreaterThan(expression, expression2);
                        break;
                    case ExpressionParser.TokenId.ExclamationEqual:
                    case ExpressionParser.TokenId.LessGreater:
                        expression = this.GenerateNotEqual(expression, expression2);
                        break;
                    case ExpressionParser.TokenId.LessThanEqual:
                        expression = this.GenerateLessThanEqual(expression, expression2);
                        break;
                    case ExpressionParser.TokenId.GreaterThanEqual:
                        expression = this.GenerateGreaterThanEqual(expression, expression2);
                        break;
                }
            }
            return expression;
        }

        private Expression ParseAdditive()
        {
            Expression expression = this.ParseMultiplicative();
            while (this.token.id == ExpressionParser.TokenId.Plus || this.token.id == ExpressionParser.TokenId.Minus || this.token.id == ExpressionParser.TokenId.Amphersand)
            {
                ExpressionParser.Token token = this.token;
                this.NextToken();
                Expression expression2 = this.ParseMultiplicative();
                ExpressionParser.TokenId id = token.id;
                if (id != ExpressionParser.TokenId.Amphersand)
                {
                    switch (id)
                    {
                        case ExpressionParser.TokenId.Plus:
                            if (!(expression.Type == typeof(string)) && !(expression2.Type == typeof(string)))
                            {
                                this.CheckAndPromoteOperands(typeof(ExpressionParser.IAddSignatures), token.text, ref expression, ref expression2, token.pos);
                                expression = this.GenerateAdd(expression, expression2);
                                continue;
                            }
                            break;
                        case ExpressionParser.TokenId.Comma:
                            continue;
                        case ExpressionParser.TokenId.Minus:
                            this.CheckAndPromoteOperands(typeof(ExpressionParser.ISubtractSignatures), token.text, ref expression, ref expression2, token.pos);
                            expression = this.GenerateSubtract(expression, expression2);
                            continue;
                        default:
                            continue;
                    }
                }
                expression = this.GenerateStringConcat(expression, expression2);
            }
            return expression;
        }

        private Expression ParseMultiplicative()
        {
            Expression expression = this.ParseUnary();
            while (this.token.id == ExpressionParser.TokenId.Asterisk || this.token.id == ExpressionParser.TokenId.Slash || this.token.id == ExpressionParser.TokenId.Percent || this.TokenIdentifierIs("mod"))
            {
                ExpressionParser.Token token = this.token;
                this.NextToken();
                Expression right = this.ParseUnary();
                this.CheckAndPromoteOperands(typeof(ExpressionParser.IArithmeticSignatures), token.text, ref expression, ref right, token.pos);
                ExpressionParser.TokenId id = token.id;
                if (id <= ExpressionParser.TokenId.Percent)
                {
                    if (id == ExpressionParser.TokenId.Identifier || id == ExpressionParser.TokenId.Percent)
                    {
                        expression = Expression.Modulo(expression, right);
                    }
                }
                else if (id != ExpressionParser.TokenId.Asterisk)
                {
                    if (id == ExpressionParser.TokenId.Slash)
                    {
                        expression = Expression.Divide(expression, right);
                    }
                }
                else
                {
                    expression = Expression.Multiply(expression, right);
                }
            }
            return expression;
        }

        private Expression ParseUnary()
        {
            if (this.token.id != ExpressionParser.TokenId.Minus && this.token.id != ExpressionParser.TokenId.Exclamation && !this.TokenIdentifierIs("not"))
            {
                return this.ParsePrimary();
            }
            ExpressionParser.Token token = this.token;
            this.NextToken();
            if (token.id == ExpressionParser.TokenId.Minus && (this.token.id == ExpressionParser.TokenId.IntegerLiteral || this.token.id == ExpressionParser.TokenId.RealLiteral))
            {
                this.token.text = "-" + this.token.text;
                this.token.pos = token.pos;
                return this.ParsePrimary();
            }
            Expression expression = this.ParseUnary();
            if (token.id == ExpressionParser.TokenId.Minus)
            {
                this.CheckAndPromoteOperand(typeof(ExpressionParser.INegationSignatures), token.text, ref expression, token.pos);
                expression = Expression.Negate(expression);
            }
            else
            {
                this.CheckAndPromoteOperand(typeof(ExpressionParser.INotSignatures), token.text, ref expression, token.pos);
                expression = Expression.Not(expression);
            }
            return expression;
        }

        private Expression ParsePrimary()
        {
            Expression expression = this.ParsePrimaryStart();
            for (; ; )
            {
                if (this.token.id == ExpressionParser.TokenId.Dot)
                {
                    this.NextToken();
                    expression = this.ParseMemberAccess(null, expression);
                }
                else
                {
                    if (this.token.id != ExpressionParser.TokenId.OpenBracket)
                    {
                        break;
                    }
                    expression = this.ParseElementAccess(expression);
                }
            }
            return expression;
        }

        private Expression ParsePrimaryStart()
        {
            switch (this.token.id)
            {
                case ExpressionParser.TokenId.Identifier:
                    return this.ParseIdentifier();
                case ExpressionParser.TokenId.StringLiteral:
                    return this.ParseStringLiteral();
                case ExpressionParser.TokenId.IntegerLiteral:
                    return this.ParseIntegerLiteral();
                case ExpressionParser.TokenId.RealLiteral:
                    return this.ParseRealLiteral();
                case ExpressionParser.TokenId.OpenParen:
                    return this.ParseParenExpression();
            }
            throw this.ParseError("Expression expected", new object[0]);
        }

        private Expression ParseStringLiteral()
        {
            this.ValidateToken(ExpressionParser.TokenId.StringLiteral);
            char c = this.token.text[0];
            string text = this.token.text.Substring(1, this.token.text.Length - 2);
            int startIndex = 0;
            for (; ; )
            {
                int num = text.IndexOf(c, startIndex);
                if (num < 0)
                {
                    break;
                }
                text = text.Remove(num, 1);
                startIndex = num + 1;
            }
            if (c != '\'')
            {
                this.NextToken();
                return this.CreateLiteral(text, text);
            }
            if (text.Length != 1)
            {
                throw this.ParseError("字符文本中的字符太多，只能包含一个字符", new object[0]);
            }
            this.NextToken();
            return this.CreateLiteral(text[0], text);
        }

        private Expression ParseIntegerLiteral()
        {
            this.ValidateToken(ExpressionParser.TokenId.IntegerLiteral);
            string text = this.token.text;
            if (text[0] != '-')
            {
                ulong num;
                if (!ulong.TryParse(text, out num))
                {
                    throw this.ParseError("无效整数“{0}”", new object[]
                    {
                        text
                    });
                }
                this.NextToken();
                if (num <= 2147483647UL)
                {
                    return this.CreateLiteral((int)num, text);
                }
                if (num <= uint.MaxValue)
                {
                    return this.CreateLiteral((uint)num, text);
                }
                if (num <= 9223372036854775807UL)
                {
                    return this.CreateLiteral((long)num, text);
                }
                return this.CreateLiteral(num, text);
            }
            else
            {
                long num2;
                if (!long.TryParse(text, out num2))
                {
                    throw this.ParseError("无效整数“{0}”", new object[]
                    {
                        text
                    });
                }
                this.NextToken();
                if (num2 >= -2147483648L && num2 <= 2147483647L)
                {
                    return this.CreateLiteral((int)num2, text);
                }
                return this.CreateLiteral(num2, text);
            }
        }

        private Expression ParseRealLiteral()
        {
            this.ValidateToken(ExpressionParser.TokenId.RealLiteral);
            string text = this.token.text;
            object obj = null;
            char c = text[text.Length - 1];
            double num2;
            if (c == 'F' || c == 'f')
            {
                float num;
                if (float.TryParse(text.Substring(0, text.Length - 1), out num))
                {
                    obj = num;
                }
            }
            else if (double.TryParse(text, out num2))
            {
                obj = num2;
            }
            if (obj == null)
            {
                throw this.ParseError("无效实数“{0}”", new object[]
                {
                    text
                });
            }
            this.NextToken();
            return this.CreateLiteral(obj, text);
        }

        private Expression CreateLiteral(object value, string text)
        {
            ConstantExpression constantExpression = Expression.Constant(value);
            this.literals.Add(constantExpression, text);
            return constantExpression;
        }

        private Expression ParseParenExpression()
        {
            this.ValidateToken(ExpressionParser.TokenId.OpenParen, "缺少“(”");
            this.NextToken();
            Expression result = this.ParseExpression();
            this.ValidateToken(ExpressionParser.TokenId.CloseParen, "缺少“)”或运算符");
            this.NextToken();
            return result;
        }

        private Expression ParseIdentifier()
        {
            this.ValidateToken(ExpressionParser.TokenId.Identifier);
            object obj;
            if (ExpressionParser.keywords.TryGetValue(this.token.text, out obj))
            {
                if (obj is Type)
                {
                    return this.ParseTypeAccess((Type)obj);
                }
                if (obj == ExpressionParser.keywordIt)
                {
                    return this.ParseIt();
                }
                if (obj == ExpressionParser.keywordIif)
                {
                    return this.ParseIif();
                }
                if (obj == ExpressionParser.keywordNew)
                {
                    return this.ParseNew();
                }
                this.NextToken();
                return (Expression)obj;
            }
            else
            {
                if (this.symbols.TryGetValue(this.token.text, out obj) || (this.externals != null && this.externals.TryGetValue(this.token.text, out obj)))
                {
                    Expression expression = obj as Expression;
                    if (expression == null)
                    {
                        expression = Expression.Constant(obj);
                    }
                    else
                    {
                        LambdaExpression lambdaExpression = expression as LambdaExpression;
                        if (lambdaExpression != null)
                        {
                            return this.ParseLambdaInvocation(lambdaExpression);
                        }
                    }
                    this.NextToken();
                    return expression;
                }
                if (this.it != null)
                {
                    return this.ParseMemberAccess(null, this.it);
                }
                throw this.ParseError("未知标识符“{0}”", new object[]
                {
                    this.token.text
                });
            }
        }

        private Expression ParseIt()
        {
            if (this.it == null)
            {
                throw this.ParseError("No 'it' is in scope", new object[0]);
            }
            this.NextToken();
            return this.it;
        }

        private Expression ParseIif()
        {
            int pos = this.token.pos;
            this.NextToken();
            Expression[] array = this.ParseArgumentList();
            if (array.Length != 3)
            {
                throw this.ParseError(pos, "“iif”方法需要2个参数", new object[0]);
            }
            return this.GenerateConditional(array[0], array[1], array[2], pos);
        }

        private Expression GenerateConditional(Expression test, Expression expr1, Expression expr2, int errorPos)
        {
            if (test.Type != typeof(bool))
            {
                throw this.ParseError(errorPos, "The first expression must be of type 'Boolean'", new object[0]);
            }
            if (expr1.Type != expr2.Type)
            {
                Expression expression = (expr2 != ExpressionParser.nullLiteral) ? this.PromoteExpression(expr1, expr2.Type, true) : null;
                Expression expression2 = (expr1 != ExpressionParser.nullLiteral) ? this.PromoteExpression(expr2, expr1.Type, true) : null;
                if (expression != null && expression2 == null)
                {
                    expr1 = expression;
                }
                else if (expression2 != null && expression == null)
                {
                    expr2 = expression2;
                }
                else
                {
                    string text = (expr1 != ExpressionParser.nullLiteral) ? expr1.Type.Name : "null";
                    string text2 = (expr2 != ExpressionParser.nullLiteral) ? expr2.Type.Name : "null";
                    if (expression != null && expression2 != null)
                    {
                        throw this.ParseError(errorPos, "Both of the types '{0}' and '{1}' convert to the other", new object[]
                        {
                            text,
                            text2
                        });
                    }
                    throw this.ParseError(errorPos, "Neither of the types '{0}' and '{1}' converts to the other", new object[]
                    {
                        text,
                        text2
                    });
                }
            }
            return Expression.Condition(test, expr1, expr2);
        }

        private Expression ParseNew()
        {
            this.NextToken();
            this.ValidateToken(ExpressionParser.TokenId.OpenParen, "缺少“(”");
            this.NextToken();
            List<DynamicProperty> list = new List<DynamicProperty>();
            List<Expression> list2 = new List<Expression>();
            int pos;
            for (; ; )
            {
                pos = this.token.pos;
                Expression expression = this.ParseExpression();
                string name;
                if (this.TokenIdentifierIs("as"))
                {
                    this.NextToken();
                    name = this.GetIdentifier();
                    this.NextToken();
                }
                else
                {
                    MemberExpression memberExpression = expression as MemberExpression;
                    if (memberExpression == null)
                    {
                        break;
                    }
                    name = memberExpression.Member.Name;
                }
                list2.Add(expression);
                list.Add(new DynamicProperty(name, expression.Type));
                if (this.token.id != ExpressionParser.TokenId.Comma)
                {
                    goto IL_BC;
                }
                this.NextToken();
            }
            throw this.ParseError(pos, "Expression is missing an 'as' clause", new object[0]);
            IL_BC:
            this.ValidateToken(ExpressionParser.TokenId.CloseParen, "缺少“)”或“,”");
            this.NextToken();
            Type type = DynamicExpression.CreateClass(list);
            MemberBinding[] array = new MemberBinding[list.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = Expression.Bind(type.GetProperty(list[i].Name), list2[i]);
            }
            return Expression.MemberInit(Expression.New(type), array);
        }

        private Expression ParseLambdaInvocation(LambdaExpression lambda)
        {
            int pos = this.token.pos;
            this.NextToken();
            Expression[] array = this.ParseArgumentList();
            MethodBase methodBase;
            if (this.FindMethod(lambda.Type, "Invoke", false, array, out methodBase) != 1)
            {
                throw this.ParseError(pos, "Argument list incompatible with lambda expression", new object[0]);
            }
            return Expression.Invoke(lambda, array);
        }

        private Expression ParseTypeAccess(Type type)
        {
            int pos = this.token.pos;
            this.NextToken();
            if (this.token.id == ExpressionParser.TokenId.Question)
            {
                if (!type.IsValueType || ExpressionParser.IsNullableType(type))
                {
                    throw this.ParseError(pos, "“{0}”必须是是不可以为 null 的值类型", new object[]
                    {
                        ExpressionParser.GetTypeName(type)
                    });
                }
                type = typeof(Nullable<>).MakeGenericType(new Type[]
                {
                    type
                });
                this.NextToken();
            }
            if (this.token.id != ExpressionParser.TokenId.OpenParen)
            {
                this.ValidateToken(ExpressionParser.TokenId.Dot, "缺少“.”或“(”");
                this.NextToken();
                return this.ParseMemberAccess(type, null);
            }
            Expression[] array = this.ParseArgumentList();
            MethodBase methodBase;
            switch (this.FindBestMethod(type.GetConstructors(), array, out methodBase))
            {
                case 0:
                    if (array.Length == 1)
                    {
                        return this.GenerateConversion(array[0], type, pos);
                    }
                    throw this.ParseError(pos, "“{0}”中未找到匹配的构造函数", new object[]
                    {
                    ExpressionParser.GetTypeName(type)
                    });
                case 1:
                    return Expression.New((ConstructorInfo)methodBase, array);
                default:
                    throw this.ParseError(pos, "Ambiguous invocation of '{0}' constructor", new object[]
                    {
                    ExpressionParser.GetTypeName(type)
                    });
            }
        }

        private Expression GenerateConversion(Expression expr, Type type, int errorPos)
        {
            Type type2 = expr.Type;
            if (type2 == type)
            {
                return expr;
            }
            if (type2.IsValueType && type.IsValueType)
            {
                if ((ExpressionParser.IsNullableType(type2) || ExpressionParser.IsNullableType(type)) && ExpressionParser.GetNonNullableType(type2) == ExpressionParser.GetNonNullableType(type))
                {
                    return Expression.Convert(expr, type);
                }
                if (((ExpressionParser.IsNumericType(type2) || ExpressionParser.IsEnumType(type2)) && ExpressionParser.IsNumericType(type)) || ExpressionParser.IsEnumType(type))
                {
                    return Expression.ConvertChecked(expr, type);
                }
            }
            if (type2.IsAssignableFrom(type) || type.IsAssignableFrom(type2) || type2.IsInterface || type.IsInterface)
            {
                return Expression.Convert(expr, type);
            }
            throw this.ParseError(errorPos, "无法将类型 “{0}” 转换为 “{1}”", new object[]
            {
                ExpressionParser.GetTypeName(type2),
                ExpressionParser.GetTypeName(type)
            });
        }

        private Expression ParseMemberAccess(Type type, Expression instance)
        {
            if (instance != null)
            {
                type = instance.Type;
            }
            int pos = this.token.pos;
            string identifier = this.GetIdentifier();
            this.NextToken();
            if (this.token.id == ExpressionParser.TokenId.OpenParen)
            {
                if (instance != null && type != typeof(string))
                {
                    Type type2 = ExpressionParser.FindGenericType(typeof(IEnumerable<>), type);
                    if (type2 != null)
                    {
                        Type elementType = type2.GetGenericArguments()[0];
                        return this.ParseAggregate(instance, elementType, identifier, pos);
                    }
                }
                Expression[] array = this.ParseArgumentList();
                MethodBase methodBase;
                switch (this.FindMethod(type, identifier, instance == null, array, out methodBase))
                {
                    case 0:
                        throw this.ParseError(pos, "No applicable method '{0}' exists in type '{1}'", new object[]
                        {
                        identifier,
                        ExpressionParser.GetTypeName(type)
                        });
                    case 1:
                        {
                            MethodInfo methodInfo = (MethodInfo)methodBase;
                            if (!ExpressionParser.IsPredefinedType(methodInfo.DeclaringType))
                            {
                                throw this.ParseError(pos, "类型“{0}”中的方法不可访问", new object[]
                                {
                            ExpressionParser.GetTypeName(methodInfo.DeclaringType)
                                });
                            }
                            if (methodInfo.ReturnType == typeof(void))
                            {
                                throw this.ParseError(pos, "类型“{1}”中的方法“{0}”没有返回值", new object[]
                                {
                            identifier,
                            ExpressionParser.GetTypeName(methodInfo.DeclaringType)
                                });
                            }
                            return Expression.Call(instance, methodInfo, array);
                        }
                    default:
                        throw this.ParseError(pos, "Ambiguous invocation of method '{0}' in type '{1}'", new object[]
                        {
                        identifier,
                        ExpressionParser.GetTypeName(type)
                        });
                }
            }
            else
            {
                MemberInfo memberInfo = this.FindPropertyOrField(type, identifier, instance == null);
                if (memberInfo == null)
                {
                    throw this.ParseError(pos, "类型“{1}”不包含名为“{0}”的属性或字段的定义", new object[]
                    {
                        identifier,
                        ExpressionParser.GetTypeName(type)
                    });
                }
                if (!(memberInfo is PropertyInfo))
                {
                    return Expression.Field(instance, (FieldInfo)memberInfo);
                }
                return Expression.Property(instance, (PropertyInfo)memberInfo);
            }
        }

        private static Type FindGenericType(Type generic, Type type)
        {
            while (type != null && type != typeof(object))
            {
                if (type.IsGenericType && type.GetGenericTypeDefinition() == generic)
                {
                    return type;
                }
                if (generic.IsInterface)
                {
                    foreach (Type type2 in type.GetInterfaces())
                    {
                        Type type3 = ExpressionParser.FindGenericType(generic, type2);
                        if (type3 != null)
                        {
                            return type3;
                        }
                    }
                }
                type = type.BaseType;
            }
            return null;
        }

        private Expression ParseAggregate(Expression instance, Type elementType, string methodName, int errorPos)
        {
            ParameterExpression parameterExpression = this.it;
            ParameterExpression parameterExpression2 = Expression.Parameter(elementType, "");
            this.it = parameterExpression2;
            Expression[] array = this.ParseArgumentList();
            this.it = parameterExpression;
            MethodBase methodBase;
            if (this.FindMethod(typeof(ExpressionParser.IEnumerableSignatures), methodName, false, array, out methodBase) != 1)
            {
                throw this.ParseError(errorPos, "不存在对应的聚合方法 “{0}”", new object[]
                {
                    methodName
                });
            }
            Type[] typeArguments;
            if (methodBase.Name == "Min" || methodBase.Name == "Max")
            {
                typeArguments = new Type[]
                {
                    elementType,
                    array[0].Type
                };
            }
            else
            {
                typeArguments = new Type[]
                {
                    elementType
                };
            }
            if (array.Length == 0)
            {
                array = new Expression[]
                {
                    instance
                };
            }
            else
            {
                array = new Expression[]
                {
                    instance,
                    Expression.Lambda(array[0], new ParameterExpression[]
                    {
                        parameterExpression2
                    })
                };
            }
            return Expression.Call(typeof(Enumerable), methodBase.Name, typeArguments, array);
        }

        private Expression[] ParseArgumentList()
        {
            this.ValidateToken(ExpressionParser.TokenId.OpenParen, "缺少“(”");
            this.NextToken();
            Expression[] result = (this.token.id != ExpressionParser.TokenId.CloseParen) ? this.ParseArguments() : new Expression[0];
            this.ValidateToken(ExpressionParser.TokenId.CloseParen, "缺少“)”或“,”");
            this.NextToken();
            return result;
        }

        private Expression[] ParseArguments()
        {
            List<Expression> list = new List<Expression>();
            for (; ; )
            {
                list.Add(this.ParseExpression());
                if (this.token.id != ExpressionParser.TokenId.Comma)
                {
                    break;
                }
                this.NextToken();
            }
            return list.ToArray();
        }

        private Expression ParseElementAccess(Expression expr)
        {
            int pos = this.token.pos;
            this.ValidateToken(ExpressionParser.TokenId.OpenBracket, "缺少“(”");
            this.NextToken();
            Expression[] array = this.ParseArguments();
            this.ValidateToken(ExpressionParser.TokenId.CloseBracket, "缺少“]”或“,”");
            this.NextToken();
            if (expr.Type.IsArray)
            {
                if (expr.Type.GetArrayRank() != 1 || array.Length != 1)
                {
                    throw this.ParseError(pos, "Indexing of multi-dimensional arrays is not supported", new object[0]);
                }
                Expression expression = this.PromoteExpression(array[0], typeof(int), true);
                if (expression == null)
                {
                    throw this.ParseError(pos, "数组索引必须是整型值", new object[0]);
                }
                return Expression.ArrayIndex(expr, expression);
            }
            else
            {
                MethodBase methodBase;
                switch (this.FindIndexer(expr.Type, array, out methodBase))
                {
                    case 0:
                        throw this.ParseError(pos, "类型“{0}”中没有适用的索引", new object[]
                        {
                        ExpressionParser.GetTypeName(expr.Type)
                        });
                    case 1:
                        return Expression.Call(expr, (MethodInfo)methodBase, array);
                    default:
                        throw this.ParseError(pos, "Ambiguous invocation of indexer in type '{0}'", new object[]
                        {
                        ExpressionParser.GetTypeName(expr.Type)
                        });
                }
            }
        }

        private static bool IsPredefinedType(Type type)
        {
            foreach (Type left in ExpressionParser.predefinedTypes)
            {
                if (left == type)
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        private static Type GetNonNullableType(Type type)
        {
            if (!ExpressionParser.IsNullableType(type))
            {
                return type;
            }
            return type.GetGenericArguments()[0];
        }

        private static string GetTypeName(Type type)
        {
            Type nonNullableType = ExpressionParser.GetNonNullableType(type);
            string text = nonNullableType.Name;
            if (type != nonNullableType)
            {
                text += '?';
            }
            return text;
        }

        private static bool IsNumericType(Type type)
        {
            return ExpressionParser.GetNumericTypeKind(type) != 0;
        }

        private static bool IsSignedIntegralType(Type type)
        {
            return ExpressionParser.GetNumericTypeKind(type) == 2;
        }

        private static bool IsUnsignedIntegralType(Type type)
        {
            return ExpressionParser.GetNumericTypeKind(type) == 3;
        }

        private static int GetNumericTypeKind(Type type)
        {
            type = ExpressionParser.GetNonNullableType(type);
            if (type.IsEnum)
            {
                return 0;
            }
            switch (Type.GetTypeCode(type))
            {
                case TypeCode.Char:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    return 1;
                case TypeCode.SByte:
                case TypeCode.Int16:
                case TypeCode.Int32:
                case TypeCode.Int64:
                    return 2;
                case TypeCode.Byte:
                case TypeCode.UInt16:
                case TypeCode.UInt32:
                case TypeCode.UInt64:
                    return 3;
                default:
                    return 0;
            }
        }

        private static bool IsEnumType(Type type)
        {
            return ExpressionParser.GetNonNullableType(type).IsEnum;
        }

        private void CheckAndPromoteOperand(Type signatures, string opName, ref Expression expr, int errorPos)
        {
            Expression[] array = new Expression[]
            {
                expr
            };
            MethodBase methodBase;
            if (this.FindMethod(signatures, "F", false, array, out methodBase) != 1)
            {
                throw this.ParseError(errorPos, "运算符“{0}”无法应用于“{1}”类型的操作数", new object[]
                {
                    opName,
                    ExpressionParser.GetTypeName(array[0].Type)
                });
            }
            expr = array[0];
        }

        // Token: 0x06000356 RID: 854 RVA: 0x0000D848 File Offset: 0x0000BA48
        private void CheckAndPromoteOperands(Type signatures, string opName, ref Expression left, ref Expression right, int errorPos)
        {
            Expression[] array = new Expression[]
            {
                left,
                right
            };
            MethodBase methodBase;
            if (this.FindMethod(signatures, "F", false, array, out methodBase) != 1)
            {
                throw this.IncompatibleOperandsError(opName, left, right, errorPos);
            }
            left = array[0];
            right = array[1];
        }

        // Token: 0x06000357 RID: 855 RVA: 0x0000D898 File Offset: 0x0000BA98
        private Exception IncompatibleOperandsError(string opName, Expression left, Expression right, int pos)
        {
            return this.ParseError(pos, "运算符“{0}”无法应用于“{1}”和“{2}”类型的操作数", new object[]
            {
                opName,
                ExpressionParser.GetTypeName(left.Type),
                ExpressionParser.GetTypeName(right.Type)
            });
        }

        // Token: 0x06000358 RID: 856 RVA: 0x0000D8DC File Offset: 0x0000BADC
        private MemberInfo FindPropertyOrField(Type type, string memberName, bool staticAccess)
        {
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            foreach (Type type2 in ExpressionParser.SelfAndBaseTypes(type))
            {
                MemberInfo[] array = type2.FindMembers(MemberTypes.Field | MemberTypes.Property, bindingAttr, Type.FilterNameIgnoreCase, memberName);
                if (array.Length != 0)
                {
                    return array[0];
                }
            }
            return null;
        }

        // Token: 0x06000359 RID: 857 RVA: 0x0000D950 File Offset: 0x0000BB50
        private int FindMethod(Type type, string methodName, bool staticAccess, Expression[] args, out MethodBase method)
        {
            BindingFlags bindingAttr = BindingFlags.DeclaredOnly | BindingFlags.Public | (staticAccess ? BindingFlags.Static : BindingFlags.Instance);
            foreach (Type type2 in ExpressionParser.SelfAndBaseTypes(type))
            {
                MemberInfo[] source = type2.FindMembers(MemberTypes.Method, bindingAttr, Type.FilterNameIgnoreCase, methodName);
                int num = this.FindBestMethod(source.Cast<MethodBase>(), args, out method);
                if (num != 0)
                {
                    return num;
                }
            }
            method = null;
            return 0;
        }

        // Token: 0x0600035A RID: 858 RVA: 0x0000D9EC File Offset: 0x0000BBEC
        private int FindIndexer(Type type, Expression[] args, out MethodBase method)
        {
            foreach (Type type2 in ExpressionParser.SelfAndBaseTypes(type))
            {
                MemberInfo[] defaultMembers = type2.GetDefaultMembers();
                if (defaultMembers.Length != 0)
                {
                    IEnumerable<MethodBase> methods = from p in defaultMembers.OfType<PropertyInfo>()
                                                      select p.GetGetMethod() into m
                                                      where m != null
                                                      select m;
                    int num = this.FindBestMethod(methods, args, out method);
                    if (num != 0)
                    {
                        return num;
                    }
                }
            }
            method = null;
            return 0;
        }

        // Token: 0x0600035B RID: 859 RVA: 0x0000DAA8 File Offset: 0x0000BCA8
        private static IEnumerable<Type> SelfAndBaseTypes(Type type)
        {
            if (type.IsInterface)
            {
                List<Type> list = new List<Type>();
                ExpressionParser.AddInterface(list, type);
                return list;
            }
            return ExpressionParser.SelfAndBaseClasses(type);
        }

        // Token: 0x0600035C RID: 860 RVA: 0x0000DBC8 File Offset: 0x0000BDC8
        private static IEnumerable<Type> SelfAndBaseClasses(Type type)
        {
            while (type != null)
            {
                yield return type;
                type = type.BaseType;
            }
            yield break;
        }

        // Token: 0x0600035D RID: 861 RVA: 0x0000DBE8 File Offset: 0x0000BDE8
        private static void AddInterface(List<Type> types, Type type)
        {
            if (!types.Contains(type))
            {
                types.Add(type);
                foreach (Type type2 in type.GetInterfaces())
                {
                    ExpressionParser.AddInterface(types, type2);
                }
            }
        }

        // Token: 0x0600035E RID: 862 RVA: 0x0000DCD0 File Offset: 0x0000BED0
        private int FindBestMethod(IEnumerable<MethodBase> methods, Expression[] args, out MethodBase method)
        {
            ExpressionParser.MethodData[] applicable = (from m in methods
                                                        select new ExpressionParser.MethodData
                                                        {
                                                            MethodBase = m,
                                                            Parameters = m.GetParameters()
                                                        } into m
                                                        where this.IsApplicable(m, args)
                                                        select m).ToArray<ExpressionParser.MethodData>();
            if (applicable.Length > 1)
            {
                applicable = (from m in applicable
                              where applicable.All((ExpressionParser.MethodData n) => m == n || ExpressionParser.IsBetterThan(args, m, n))
                              select m).ToArray<ExpressionParser.MethodData>();
            }
            if (applicable.Length == 1)
            {
                ExpressionParser.MethodData methodData = applicable[0];
                for (int i = 0; i < args.Length; i++)
                {
                    args[i] = methodData.Args[i];
                }
                method = methodData.MethodBase;
            }
            else
            {
                method = null;
            }
            return applicable.Length;
        }

        // Token: 0x0600035F RID: 863 RVA: 0x0000DDB0 File Offset: 0x0000BFB0
        private bool IsApplicable(ExpressionParser.MethodData method, Expression[] args)
        {
            if (method.Parameters.Length != args.Length)
            {
                return false;
            }
            Expression[] array = new Expression[args.Length];
            for (int i = 0; i < args.Length; i++)
            {
                ParameterInfo parameterInfo = method.Parameters[i];
                if (parameterInfo.IsOut)
                {
                    return false;
                }
                Expression expression = this.PromoteExpression(args[i], parameterInfo.ParameterType, false);
                if (expression == null)
                {
                    return false;
                }
                array[i] = expression;
            }
            method.Args = array;
            return true;
        }

        // Token: 0x06000360 RID: 864 RVA: 0x0000DE18 File Offset: 0x0000C018
        private Expression PromoteExpression(Expression expr, Type type, bool exact)
        {
            if (expr.Type == type)
            {
                return expr;
            }
            if (expr is ConstantExpression)
            {
                ConstantExpression constantExpression = (ConstantExpression)expr;
                string name;
                if (constantExpression == ExpressionParser.nullLiteral)
                {
                    if (!type.IsValueType || ExpressionParser.IsNullableType(type))
                    {
                        return Expression.Constant(null, type);
                    }
                }
                else if (this.literals.TryGetValue(constantExpression, out name))
                {
                    Type nonNullableType = ExpressionParser.GetNonNullableType(type);
                    object obj = null;
                    switch (Type.GetTypeCode(constantExpression.Type))
                    {
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                            obj = ExpressionParser.ParseNumber(name, nonNullableType);
                            break;
                        case TypeCode.Double:
                            if (nonNullableType == typeof(decimal))
                            {
                                obj = ExpressionParser.ParseNumber(name, nonNullableType);
                            }
                            break;
                        case TypeCode.String:
                            obj = ExpressionParser.ParseEnum(name, nonNullableType);
                            break;
                    }
                    if (obj != null)
                    {
                        return Expression.Constant(obj, type);
                    }
                }
            }
            if (!ExpressionParser.IsCompatibleWith(expr.Type, type))
            {
                return null;
            }
            if (type.IsValueType || exact)
            {
                return Expression.Convert(expr, type);
            }
            return expr;
        }

        // Token: 0x06000361 RID: 865 RVA: 0x0000DF24 File Offset: 0x0000C124
        private static object ParseNumber(string text, Type type)
        {
            switch (Type.GetTypeCode(ExpressionParser.GetNonNullableType(type)))
            {
                case TypeCode.SByte:
                    {
                        sbyte b;
                        if (sbyte.TryParse(text, out b))
                        {
                            return b;
                        }
                        break;
                    }
                case TypeCode.Byte:
                    {
                        byte b2;
                        if (byte.TryParse(text, out b2))
                        {
                            return b2;
                        }
                        break;
                    }
                case TypeCode.Int16:
                    {
                        short num;
                        if (short.TryParse(text, out num))
                        {
                            return num;
                        }
                        break;
                    }
                case TypeCode.UInt16:
                    {
                        ushort num2;
                        if (ushort.TryParse(text, out num2))
                        {
                            return num2;
                        }
                        break;
                    }
                case TypeCode.Int32:
                    {
                        int num3;
                        if (int.TryParse(text, out num3))
                        {
                            return num3;
                        }
                        break;
                    }
                case TypeCode.UInt32:
                    {
                        uint num4;
                        if (uint.TryParse(text, out num4))
                        {
                            return num4;
                        }
                        break;
                    }
                case TypeCode.Int64:
                    {
                        long num5;
                        if (long.TryParse(text, out num5))
                        {
                            return num5;
                        }
                        break;
                    }
                case TypeCode.UInt64:
                    {
                        ulong num6;
                        if (ulong.TryParse(text, out num6))
                        {
                            return num6;
                        }
                        break;
                    }
                case TypeCode.Single:
                    {
                        float num7;
                        if (float.TryParse(text, out num7))
                        {
                            return num7;
                        }
                        break;
                    }
                case TypeCode.Double:
                    {
                        double num8;
                        if (double.TryParse(text, out num8))
                        {
                            return num8;
                        }
                        break;
                    }
                case TypeCode.Decimal:
                    {
                        decimal num9;
                        if (decimal.TryParse(text, out num9))
                        {
                            return num9;
                        }
                        break;
                    }
            }
            return null;
        }

        // Token: 0x06000362 RID: 866 RVA: 0x0000E048 File Offset: 0x0000C248
        private static object ParseEnum(string name, Type type)
        {
            if (type.IsEnum)
            {
                MemberInfo[] array = type.FindMembers(MemberTypes.Field, BindingFlags.DeclaredOnly | BindingFlags.Static | BindingFlags.Public, Type.FilterNameIgnoreCase, name);
                if (array.Length != 0)
                {
                    return ((FieldInfo)array[0]).GetValue(null);
                }
            }
            return null;
        }

        // Token: 0x06000363 RID: 867 RVA: 0x0000E084 File Offset: 0x0000C284
        private static bool IsCompatibleWith(Type source, Type target)
        {
            if (source == target)
            {
                return true;
            }
            if (!target.IsValueType)
            {
                return target.IsAssignableFrom(source);
            }
            Type nonNullableType = ExpressionParser.GetNonNullableType(source);
            Type nonNullableType2 = ExpressionParser.GetNonNullableType(target);
            if (nonNullableType != source && nonNullableType2 == target)
            {
                return false;
            }
            TypeCode typeCode = nonNullableType.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType);
            TypeCode typeCode2 = nonNullableType2.IsEnum ? TypeCode.Object : Type.GetTypeCode(nonNullableType2);
            switch (typeCode)
            {
                case TypeCode.SByte:
                    switch (typeCode2)
                    {
                        case TypeCode.SByte:
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Byte:
                    switch (typeCode2)
                    {
                        case TypeCode.Byte:
                        case TypeCode.Int16:
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Int16:
                    switch (typeCode2)
                    {
                        case TypeCode.Int16:
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.UInt16:
                    switch (typeCode2)
                    {
                        case TypeCode.UInt16:
                        case TypeCode.Int32:
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Int32:
                    switch (typeCode2)
                    {
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.UInt32:
                    switch (typeCode2)
                    {
                        case TypeCode.UInt32:
                        case TypeCode.Int64:
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Int64:
                    switch (typeCode2)
                    {
                        case TypeCode.Int64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.UInt64:
                    switch (typeCode2)
                    {
                        case TypeCode.UInt64:
                        case TypeCode.Single:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            return true;
                    }
                    break;
                case TypeCode.Single:
                    switch (typeCode2)
                    {
                        case TypeCode.Single:
                        case TypeCode.Double:
                            return true;
                    }
                    break;
                default:
                    if (nonNullableType == nonNullableType2)
                    {
                        return true;
                    }
                    break;
            }
            return false;
        }

        // Token: 0x06000364 RID: 868 RVA: 0x0000E2D0 File Offset: 0x0000C4D0
        private static bool IsBetterThan(Expression[] args, ExpressionParser.MethodData m1, ExpressionParser.MethodData m2)
        {
            bool result = false;
            for (int i = 0; i < args.Length; i++)
            {
                int num = ExpressionParser.CompareConversions(args[i].Type, m1.Parameters[i].ParameterType, m2.Parameters[i].ParameterType);
                if (num < 0)
                {
                    return false;
                }
                if (num > 0)
                {
                    result = true;
                }
            }
            return result;
        }

        // Token: 0x06000365 RID: 869 RVA: 0x0000E324 File Offset: 0x0000C524
        private static int CompareConversions(Type s, Type t1, Type t2)
        {
            if (t1 == t2)
            {
                return 0;
            }
            if (s == t1)
            {
                return 1;
            }
            if (s == t2)
            {
                return -1;
            }
            bool flag = ExpressionParser.IsCompatibleWith(t1, t2);
            bool flag2 = ExpressionParser.IsCompatibleWith(t2, t1);
            if (flag && !flag2)
            {
                return 1;
            }
            if (flag2 && !flag)
            {
                return -1;
            }
            if (ExpressionParser.IsSignedIntegralType(t1) && ExpressionParser.IsUnsignedIntegralType(t2))
            {
                return 1;
            }
            if (ExpressionParser.IsSignedIntegralType(t2) && ExpressionParser.IsUnsignedIntegralType(t1))
            {
                return -1;
            }
            return 0;
        }

        // Token: 0x06000366 RID: 870 RVA: 0x0000E397 File Offset: 0x0000C597
        private Expression GenerateEqual(Expression left, Expression right)
        {
            return Expression.Equal(left, right);
        }

        // Token: 0x06000367 RID: 871 RVA: 0x0000E3A0 File Offset: 0x0000C5A0
        private Expression GenerateNotEqual(Expression left, Expression right)
        {
            return Expression.NotEqual(left, right);
        }

        // Token: 0x06000368 RID: 872 RVA: 0x0000E3A9 File Offset: 0x0000C5A9
        private Expression GenerateGreaterThan(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThan(this.GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
            }
            return Expression.GreaterThan(left, right);
        }

        // Token: 0x06000369 RID: 873 RVA: 0x0000E3E7 File Offset: 0x0000C5E7
        private Expression GenerateGreaterThanEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.GreaterThanOrEqual(this.GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
            }
            return Expression.GreaterThanOrEqual(left, right);
        }

        // Token: 0x0600036A RID: 874 RVA: 0x0000E425 File Offset: 0x0000C625
        private Expression GenerateLessThan(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThan(this.GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
            }
            return Expression.LessThan(left, right);
        }

        // Token: 0x0600036B RID: 875 RVA: 0x0000E463 File Offset: 0x0000C663
        private Expression GenerateLessThanEqual(Expression left, Expression right)
        {
            if (left.Type == typeof(string))
            {
                return Expression.LessThanOrEqual(this.GenerateStaticMethodCall("Compare", left, right), Expression.Constant(0));
            }
            return Expression.LessThanOrEqual(left, right);
        }

        // Token: 0x0600036C RID: 876 RVA: 0x0000E4A4 File Offset: 0x0000C6A4
        private Expression GenerateAdd(Expression left, Expression right)
        {
            if (left.Type == typeof(string) && right.Type == typeof(string))
            {
                return this.GenerateStaticMethodCall("Concat", left, right);
            }
            return Expression.Add(left, right);
        }

        // Token: 0x0600036D RID: 877 RVA: 0x0000E4F4 File Offset: 0x0000C6F4
        private Expression GenerateSubtract(Expression left, Expression right)
        {
            return Expression.Subtract(left, right);
        }

        // Token: 0x0600036E RID: 878 RVA: 0x0000E500 File Offset: 0x0000C700
        private Expression GenerateStringConcat(Expression left, Expression right)
        {
            return Expression.Call(null, typeof(string).GetMethod("Concat", new Type[]
            {
                typeof(object),
                typeof(object)
            }), new Expression[]
            {
                left,
                right
            });
        }

        // Token: 0x0600036F RID: 879 RVA: 0x0000E55C File Offset: 0x0000C75C
        private MethodInfo GetStaticMethod(string methodName, Expression left, Expression right)
        {
            return left.Type.GetMethod(methodName, new Type[]
            {
                left.Type,
                right.Type
            });
        }

        // Token: 0x06000370 RID: 880 RVA: 0x0000E590 File Offset: 0x0000C790
        private Expression GenerateStaticMethodCall(string methodName, Expression left, Expression right)
        {
            return Expression.Call(null, this.GetStaticMethod(methodName, left, right), new Expression[]
            {
                left,
                right
            });
        }

        // Token: 0x06000371 RID: 881 RVA: 0x0000E5BC File Offset: 0x0000C7BC
        private void SetTextPos(int pos)
        {
            this.textPos = pos;
            this.ch = ((this.textPos < this.textLen) ? this.text[this.textPos] : '\0');
        }

        // Token: 0x06000372 RID: 882 RVA: 0x0000E5F0 File Offset: 0x0000C7F0
        private void NextChar()
        {
            if (this.textPos < this.textLen)
            {
                this.textPos++;
            }
            this.ch = ((this.textPos < this.textLen) ? this.text[this.textPos] : '\0');
        }

        // Token: 0x06000373 RID: 883 RVA: 0x0000E644 File Offset: 0x0000C844
        private void NextToken()
        {
            while (char.IsWhiteSpace(this.ch))
            {
                this.NextChar();
            }
            int num = this.textPos;
            char c = this.ch;
            ExpressionParser.TokenId id;
            switch (c)
            {
                case '!':
                    this.NextChar();
                    if (this.ch == '=')
                    {
                        this.NextChar();
                        id = ExpressionParser.TokenId.ExclamationEqual;
                        goto IL_41E;
                    }
                    id = ExpressionParser.TokenId.Exclamation;
                    goto IL_41E;
                case '"':
                case '\'':
                    {
                        char c2 = this.ch;
                        for (; ; )
                        {
                            this.NextChar();
                            while (this.textPos < this.textLen && this.ch != c2)
                            {
                                this.NextChar();
                            }
                            if (this.textPos == this.textLen)
                            {
                                break;
                            }
                            this.NextChar();
                            if (this.ch != c2)
                            {
                                goto Block_14;
                            }
                        }
                        throw this.ParseError(this.textPos, "未结束的字符串表达式", new object[0]);
                        Block_14:
                        id = ExpressionParser.TokenId.StringLiteral;
                        goto IL_41E;
                    }
                case '#':
                case '$':
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case '9':
                case ';':
                    break;
                case '%':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Percent;
                    goto IL_41E;
                case '&':
                    this.NextChar();
                    if (this.ch == '&')
                    {
                        this.NextChar();
                        id = ExpressionParser.TokenId.DoubleAmphersand;
                        goto IL_41E;
                    }
                    id = ExpressionParser.TokenId.Amphersand;
                    goto IL_41E;
                case '(':
                    this.NextChar();
                    id = ExpressionParser.TokenId.OpenParen;
                    goto IL_41E;
                case ')':
                    this.NextChar();
                    id = ExpressionParser.TokenId.CloseParen;
                    goto IL_41E;
                case '*':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Asterisk;
                    goto IL_41E;
                case '+':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Plus;
                    goto IL_41E;
                case ',':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Comma;
                    goto IL_41E;
                case '-':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Minus;
                    goto IL_41E;
                case '.':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Dot;
                    goto IL_41E;
                case '/':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Slash;
                    goto IL_41E;
                case ':':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Colon;
                    goto IL_41E;
                case '<':
                    this.NextChar();
                    if (this.ch == '=')
                    {
                        this.NextChar();
                        id = ExpressionParser.TokenId.LessThanEqual;
                        goto IL_41E;
                    }
                    if (this.ch == '>')
                    {
                        this.NextChar();
                        id = ExpressionParser.TokenId.LessGreater;
                        goto IL_41E;
                    }
                    id = ExpressionParser.TokenId.LessThan;
                    goto IL_41E;
                case '=':
                    this.NextChar();
                    if (this.ch == '=')
                    {
                        this.NextChar();
                        id = ExpressionParser.TokenId.DoubleEqual;
                        goto IL_41E;
                    }
                    id = ExpressionParser.TokenId.Equal;
                    goto IL_41E;
                case '>':
                    this.NextChar();
                    if (this.ch == '=')
                    {
                        this.NextChar();
                        id = ExpressionParser.TokenId.GreaterThanEqual;
                        goto IL_41E;
                    }
                    id = ExpressionParser.TokenId.GreaterThan;
                    goto IL_41E;
                case '?':
                    this.NextChar();
                    id = ExpressionParser.TokenId.Question;
                    goto IL_41E;
                default:
                    switch (c)
                    {
                        case '[':
                            this.NextChar();
                            id = ExpressionParser.TokenId.OpenBracket;
                            goto IL_41E;
                        case '\\':
                            break;
                        case ']':
                            this.NextChar();
                            id = ExpressionParser.TokenId.CloseBracket;
                            goto IL_41E;
                        default:
                            if (c == '|')
                            {
                                this.NextChar();
                                if (this.ch == '|')
                                {
                                    this.NextChar();
                                    id = ExpressionParser.TokenId.DoubleBar;
                                    goto IL_41E;
                                }
                                id = ExpressionParser.TokenId.Bar;
                                goto IL_41E;
                            }
                            break;
                    }
                    break;
            }
            if (char.IsLetter(this.ch) || this.ch == '@' || this.ch == '_')
            {
                do
                {
                    this.NextChar();
                }
                while (char.IsLetterOrDigit(this.ch) || this.ch == '_');
                id = ExpressionParser.TokenId.Identifier;
            }
            else if (char.IsDigit(this.ch))
            {
                id = ExpressionParser.TokenId.IntegerLiteral;
                do
                {
                    this.NextChar();
                }
                while (char.IsDigit(this.ch));
                if (this.ch == '.')
                {
                    id = ExpressionParser.TokenId.RealLiteral;
                    this.NextChar();
                    this.ValidateDigit();
                    do
                    {
                        this.NextChar();
                    }
                    while (char.IsDigit(this.ch));
                }
                if (this.ch == 'E' || this.ch == 'e')
                {
                    id = ExpressionParser.TokenId.RealLiteral;
                    this.NextChar();
                    if (this.ch == '+' || this.ch == '-')
                    {
                        this.NextChar();
                    }
                    this.ValidateDigit();
                    do
                    {
                        this.NextChar();
                    }
                    while (char.IsDigit(this.ch));
                }
                if (this.ch == 'F' || this.ch == 'f')
                {
                    this.NextChar();
                }
            }
            else
            {
                if (this.textPos != this.textLen)
                {
                    throw this.ParseError(this.textPos, "语法错误 “{0}”", new object[]
                    {
                        this.ch
                    });
                }
                id = ExpressionParser.TokenId.End;
            }
            IL_41E:
            this.token.id = id;
            this.token.text = this.text.Substring(num, this.textPos - num);
            this.token.pos = num;
        }

        // Token: 0x06000374 RID: 884 RVA: 0x0000EAA6 File Offset: 0x0000CCA6
        private bool TokenIdentifierIs(string id)
        {
            return this.token.id == ExpressionParser.TokenId.Identifier && string.Equals(id, this.token.text, StringComparison.OrdinalIgnoreCase);
        }

        // Token: 0x06000375 RID: 885 RVA: 0x0000EACC File Offset: 0x0000CCCC
        private string GetIdentifier()
        {
            this.ValidateToken(ExpressionParser.TokenId.Identifier, "缺少标识符");
            string text = this.token.text;
            if (text.Length > 1 && text[0] == '@')
            {
                text = text.Substring(1);
            }
            return text;
        }

        // Token: 0x06000376 RID: 886 RVA: 0x0000EB0E File Offset: 0x0000CD0E
        private void ValidateDigit()
        {
            if (!char.IsDigit(this.ch))
            {
                throw this.ParseError(this.textPos, "Digit expected", new object[0]);
            }
        }

        // Token: 0x06000377 RID: 887 RVA: 0x0000EB35 File Offset: 0x0000CD35
        private void ValidateToken(ExpressionParser.TokenId t, string errorMessage)
        {
            if (this.token.id != t)
            {
                throw this.ParseError(errorMessage, new object[0]);
            }
        }

        // Token: 0x06000378 RID: 888 RVA: 0x0000EB53 File Offset: 0x0000CD53
        private void ValidateToken(ExpressionParser.TokenId t)
        {
            if (this.token.id != t)
            {
                throw this.ParseError("语法错误", new object[0]);
            }
        }

        private Exception ParseError(string format, params object[] args)
        {
            return this.ParseError(this.token.pos, format, args);
        }

        private Exception ParseError(int pos, string format, params object[] args)
        {
            return new ParseException(string.Format(CultureInfo.CurrentCulture, format, args), pos);
        }

        private static Dictionary<string, object> CreateKeywords()
        {
            Dictionary<string, object> dictionary = new Dictionary<string, object>(StringComparer.Ordinal);
            dictionary.Add("true", ExpressionParser.trueLiteral);
            dictionary.Add("false", ExpressionParser.falseLiteral);
            dictionary.Add("null", ExpressionParser.nullLiteral);
            dictionary.Add(ExpressionParser.keywordIt, ExpressionParser.keywordIt);
            dictionary.Add(ExpressionParser.keywordIif, ExpressionParser.keywordIif);
            dictionary.Add(ExpressionParser.keywordNew, ExpressionParser.keywordNew);
            foreach (Type type in ExpressionParser.predefinedTypes)
            {
                dictionary.Add(type.Name, type);
            }
            return dictionary;
        }

        private static readonly Type[] predefinedTypes = new Type[]
        {
            typeof(object),
            typeof(bool),
            typeof(char),
            typeof(string),
            typeof(sbyte),
            typeof(byte),
            typeof(short),
            typeof(ushort),
            typeof(int),
            typeof(uint),
            typeof(long),
            typeof(ulong),
            typeof(float),
            typeof(double),
            typeof(decimal),
            typeof(DateTime),
            typeof(TimeSpan),
            typeof(Guid),
            typeof(Math),
            typeof(Geometry),
            typeof(Convert)
        };

        // Token: 0x04000114 RID: 276
        private static readonly Expression trueLiteral = Expression.Constant(true);

        // Token: 0x04000115 RID: 277
        private static readonly Expression falseLiteral = Expression.Constant(false);

        // Token: 0x04000116 RID: 278
        private static readonly Expression nullLiteral = Expression.Constant(null);

        // Token: 0x04000117 RID: 279
        private static readonly string keywordIt = "it";

        // Token: 0x04000118 RID: 280
        private static readonly string keywordIif = "iif";

        // Token: 0x04000119 RID: 281
        private static readonly string keywordNew = "new";

        // Token: 0x0400011A RID: 282
        private static Dictionary<string, object> keywords;

        // Token: 0x0400011B RID: 283
        private Dictionary<string, object> symbols;

        // Token: 0x0400011C RID: 284
        private IDictionary<string, object> externals;

        // Token: 0x0400011D RID: 285
        private Dictionary<Expression, string> literals;

        // Token: 0x0400011E RID: 286
        private ParameterExpression it;

        // Token: 0x0400011F RID: 287
        private string text;

        // Token: 0x04000120 RID: 288
        private int textPos;

        // Token: 0x04000121 RID: 289
        private int textLen;

        // Token: 0x04000122 RID: 290
        private char ch;

        // Token: 0x04000123 RID: 291
        private ExpressionParser.Token token;

        // Token: 0x0200007C RID: 124
        private struct Token
        {
            // Token: 0x04000127 RID: 295
            public ExpressionParser.TokenId id;

            // Token: 0x04000128 RID: 296
            public string text;

            // Token: 0x04000129 RID: 297
            public int pos;
        }

        // Token: 0x0200007D RID: 125
        private enum TokenId
        {
            // Token: 0x0400012B RID: 299
            Unknown,
            // Token: 0x0400012C RID: 300
            End,
            // Token: 0x0400012D RID: 301
            Identifier,
            // Token: 0x0400012E RID: 302
            StringLiteral,
            // Token: 0x0400012F RID: 303
            IntegerLiteral,
            // Token: 0x04000130 RID: 304
            RealLiteral,
            // Token: 0x04000131 RID: 305
            Exclamation,
            // Token: 0x04000132 RID: 306
            Percent,
            // Token: 0x04000133 RID: 307
            Amphersand,
            // Token: 0x04000134 RID: 308
            OpenParen,
            // Token: 0x04000135 RID: 309
            CloseParen,
            // Token: 0x04000136 RID: 310
            Asterisk,
            // Token: 0x04000137 RID: 311
            Plus,
            // Token: 0x04000138 RID: 312
            Comma,
            // Token: 0x04000139 RID: 313
            Minus,
            // Token: 0x0400013A RID: 314
            Dot,
            // Token: 0x0400013B RID: 315
            Slash,
            // Token: 0x0400013C RID: 316
            Colon,
            // Token: 0x0400013D RID: 317
            LessThan,
            // Token: 0x0400013E RID: 318
            Equal,
            // Token: 0x0400013F RID: 319
            GreaterThan,
            // Token: 0x04000140 RID: 320
            Question,
            // Token: 0x04000141 RID: 321
            OpenBracket,
            // Token: 0x04000142 RID: 322
            CloseBracket,
            // Token: 0x04000143 RID: 323
            Bar,
            // Token: 0x04000144 RID: 324
            ExclamationEqual,
            // Token: 0x04000145 RID: 325
            DoubleAmphersand,
            // Token: 0x04000146 RID: 326
            LessThanEqual,
            // Token: 0x04000147 RID: 327
            LessGreater,
            // Token: 0x04000148 RID: 328
            DoubleEqual,
            // Token: 0x04000149 RID: 329
            GreaterThanEqual,
            // Token: 0x0400014A RID: 330
            DoubleBar
        }

        // Token: 0x0200007E RID: 126
        private interface ILogicalSignatures
        {
            // Token: 0x06000380 RID: 896
            void F(bool x, bool y);

            // Token: 0x06000381 RID: 897
            void F(bool? x, bool? y);
        }

        // Token: 0x0200007F RID: 127
        private interface IArithmeticSignatures
        {
            // Token: 0x06000382 RID: 898
            void F(int x, int y);

            // Token: 0x06000383 RID: 899
            void F(uint x, uint y);

            // Token: 0x06000384 RID: 900
            void F(long x, long y);

            // Token: 0x06000385 RID: 901
            void F(ulong x, ulong y);

            // Token: 0x06000386 RID: 902
            void F(float x, float y);

            // Token: 0x06000387 RID: 903
            void F(double x, double y);

            // Token: 0x06000388 RID: 904
            void F(decimal x, decimal y);

            // Token: 0x06000389 RID: 905
            void F(int? x, int? y);

            // Token: 0x0600038A RID: 906
            void F(uint? x, uint? y);

            // Token: 0x0600038B RID: 907
            void F(long? x, long? y);

            // Token: 0x0600038C RID: 908
            void F(ulong? x, ulong? y);

            // Token: 0x0600038D RID: 909
            void F(float? x, float? y);

            // Token: 0x0600038E RID: 910
            void F(double? x, double? y);

            // Token: 0x0600038F RID: 911
            void F(decimal? x, decimal? y);

            // Token: 0x06000390 RID: 912
            void F(int? x, object y);

            // Token: 0x06000391 RID: 913
            void F(uint? x, object y);

            // Token: 0x06000392 RID: 914
            void F(long? x, object y);

            // Token: 0x06000393 RID: 915
            void F(ulong? x, object y);

            // Token: 0x06000394 RID: 916
            void F(float? x, object y);

            // Token: 0x06000395 RID: 917
            void F(double? x, object y);

            // Token: 0x06000396 RID: 918
            void F(decimal? x, object y);
        }

        // Token: 0x02000080 RID: 128
        private interface IRelationalSignatures : ExpressionParser.IArithmeticSignatures
        {
            // Token: 0x06000397 RID: 919
            void F(string x, string y);

            // Token: 0x06000398 RID: 920
            void F(char x, char y);

            // Token: 0x06000399 RID: 921
            void F(DateTime x, DateTime y);

            // Token: 0x0600039A RID: 922
            void F(TimeSpan x, TimeSpan y);

            // Token: 0x0600039B RID: 923
            void F(char? x, char? y);

            // Token: 0x0600039C RID: 924
            void F(DateTime? x, DateTime? y);

            // Token: 0x0600039D RID: 925
            void F(TimeSpan? x, TimeSpan? y);

            // Token: 0x0600039E RID: 926
            void F(char? x, object y);

            // Token: 0x0600039F RID: 927
            void F(DateTime? x, object y);

            // Token: 0x060003A0 RID: 928
            void F(TimeSpan? x, object y);
        }

        // Token: 0x02000081 RID: 129
        private interface IEqualitySignatures : ExpressionParser.IRelationalSignatures, ExpressionParser.IArithmeticSignatures
        {
            // Token: 0x060003A1 RID: 929
            void F(bool x, bool y);

            // Token: 0x060003A2 RID: 930
            void F(bool? x, bool? y);

            // Token: 0x060003A3 RID: 931
            void F(Guid x, Guid y);

            // Token: 0x060003A4 RID: 932
            void F(Guid? x, Guid? y);

            // Token: 0x060003A5 RID: 933
            void F(bool? x, object y);

            // Token: 0x060003A6 RID: 934
            void F(Guid? x, object y);
        }

        // Token: 0x02000082 RID: 130
        private interface IAddSignatures : ExpressionParser.IArithmeticSignatures
        {
            // Token: 0x060003A7 RID: 935
            void F(DateTime x, TimeSpan y);

            // Token: 0x060003A8 RID: 936
            void F(TimeSpan x, TimeSpan y);

            // Token: 0x060003A9 RID: 937
            void F(DateTime? x, TimeSpan? y);

            // Token: 0x060003AA RID: 938
            void F(TimeSpan? x, TimeSpan? y);
        }

        // Token: 0x02000083 RID: 131
        private interface ISubtractSignatures : ExpressionParser.IAddSignatures, ExpressionParser.IArithmeticSignatures
        {
            // Token: 0x060003AB RID: 939
            void F(DateTime x, DateTime y);

            // Token: 0x060003AC RID: 940
            void F(DateTime? x, DateTime? y);
        }

        // Token: 0x02000084 RID: 132
        private interface INegationSignatures
        {
            // Token: 0x060003AD RID: 941
            void F(int x);

            // Token: 0x060003AE RID: 942
            void F(long x);

            // Token: 0x060003AF RID: 943
            void F(float x);

            // Token: 0x060003B0 RID: 944
            void F(double x);

            // Token: 0x060003B1 RID: 945
            void F(decimal x);

            // Token: 0x060003B2 RID: 946
            void F(int? x);

            // Token: 0x060003B3 RID: 947
            void F(long? x);

            // Token: 0x060003B4 RID: 948
            void F(float? x);

            // Token: 0x060003B5 RID: 949
            void F(double? x);

            // Token: 0x060003B6 RID: 950
            void F(decimal? x);
        }

        // Token: 0x02000085 RID: 133
        private interface INotSignatures
        {
            // Token: 0x060003B7 RID: 951
            void F(bool x);

            // Token: 0x060003B8 RID: 952
            void F(bool? x);
        }

        // Token: 0x02000086 RID: 134
        private interface IEnumerableSignatures
        {
            // Token: 0x060003B9 RID: 953
            void Where(bool predicate);

            // Token: 0x060003BA RID: 954
            void Any();

            // Token: 0x060003BB RID: 955
            void Any(bool predicate);

            // Token: 0x060003BC RID: 956
            void All(bool predicate);

            // Token: 0x060003BD RID: 957
            void Count();

            // Token: 0x060003BE RID: 958
            void Count(bool predicate);

            // Token: 0x060003BF RID: 959
            void Min(object selector);

            // Token: 0x060003C0 RID: 960
            void Max(object selector);

            // Token: 0x060003C1 RID: 961
            void Sum(int selector);

            // Token: 0x060003C2 RID: 962
            void Sum(int? selector);

            // Token: 0x060003C3 RID: 963
            void Sum(long selector);

            // Token: 0x060003C4 RID: 964
            void Sum(long? selector);

            // Token: 0x060003C5 RID: 965
            void Sum(float selector);

            // Token: 0x060003C6 RID: 966
            void Sum(float? selector);

            // Token: 0x060003C7 RID: 967
            void Sum(double selector);

            // Token: 0x060003C8 RID: 968
            void Sum(double? selector);

            // Token: 0x060003C9 RID: 969
            void Sum(decimal selector);

            // Token: 0x060003CA RID: 970
            void Sum(decimal? selector);

            // Token: 0x060003CB RID: 971
            void Average(int selector);

            // Token: 0x060003CC RID: 972
            void Average(int? selector);

            // Token: 0x060003CD RID: 973
            void Average(long selector);

            // Token: 0x060003CE RID: 974
            void Average(long? selector);

            // Token: 0x060003CF RID: 975
            void Average(float selector);

            // Token: 0x060003D0 RID: 976
            void Average(float? selector);

            // Token: 0x060003D1 RID: 977
            void Average(double selector);

            // Token: 0x060003D2 RID: 978
            void Average(double? selector);

            // Token: 0x060003D3 RID: 979
            void Average(decimal selector);

            // Token: 0x060003D4 RID: 980
            void Average(decimal? selector);
        }

        // Token: 0x02000087 RID: 135
        private class MethodData
        {
            // Token: 0x0400014B RID: 331
            public MethodBase MethodBase;

            // Token: 0x0400014C RID: 332
            public ParameterInfo[] Parameters;

            // Token: 0x0400014D RID: 333
            public Expression[] Args;
        }
    }
}
