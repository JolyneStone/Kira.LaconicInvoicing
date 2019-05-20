using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Kira.LaconicInvoicing
{
    public static class DynamicQueryable
    {
        static DynamicQueryable()
        {
            MethodInfo[] methods = typeof(IQueryProvider).GetMethods();
            foreach (MethodInfo methodInfo in methods)
            {
                if (methodInfo.IsGenericMethod && methodInfo.Name == "CreateQuery")
                {
                    DynamicQueryable.miCreateQuery = methodInfo;
                    break;
                }
            }
            DynamicQueryable.miToList = typeof(Enumerable).GetMethod("ToList");
        }

        public static IQueryable<T> Where<T>(this IQueryable<T> source, string predicate, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(predicate))
            {
                return source;
            }
            LambdaExpression expression = DynamicExpression.ParseLambda(source.ElementType, typeof(bool), predicate, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable<T>)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Where", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(expression)
                })
            });
        }

        public static IQueryable Where(this IQueryable source, string predicate, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(predicate))
            {
                throw new ArgumentNullException("predicate");
            }
            LambdaExpression expression = DynamicExpression.ParseLambda(source.ElementType, typeof(bool), predicate, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Where", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(expression)
                })
            });
        }

        public static IQueryable Select(this IQueryable source, string selector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, selector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Select", new Type[]
                {
                    source.ElementType,
                    lambdaExpression.Body.Type
                }, new Expression[]
                {
                    source.Expression,
                    lambdaExpression
                })
            });
        }

        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (string.IsNullOrWhiteSpace(ordering))
            {
                return source;
            }
            ParameterExpression[] parameters = new ParameterExpression[]
            {
                Expression.Parameter(source.ElementType, "")
            };
            ExpressionParser expressionParser = new ExpressionParser(parameters, ordering, values, false);
            IEnumerable<DynamicOrdering> enumerable = expressionParser.ParseOrdering();
            Expression expression = source.Expression;
            string text = "OrderBy";
            string text2 = "OrderByDescending";
            foreach (DynamicOrdering dynamicOrdering in enumerable)
            {
                expression = Expression.Call(typeof(Queryable), dynamicOrdering.Ascending ? text : text2, new Type[]
                {
                    source.ElementType,
                    dynamicOrdering.Selector.Type
                }, new Expression[]
                {
                    expression,
                    Expression.Quote(Expression.Lambda(dynamicOrdering.Selector, parameters))
                });
                text = "ThenBy";
                text2 = "ThenByDescending";
            }
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable<T>)methodInfo.Invoke(source.Provider, new object[]
            {
                expression
            });
        }

        public static IQueryable OrderBy(this IQueryable source, string ordering, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (ordering == null)
            {
                throw new ArgumentNullException("ordering");
            }
            ParameterExpression[] parameters = new ParameterExpression[]
            {
                Expression.Parameter(source.ElementType, "")
            };
            ExpressionParser expressionParser = new ExpressionParser(parameters, ordering, values, false);
            IEnumerable<DynamicOrdering> enumerable = expressionParser.ParseOrdering();
            Expression expression = source.Expression;
            string text = "OrderBy";
            string text2 = "OrderByDescending";
            foreach (DynamicOrdering dynamicOrdering in enumerable)
            {
                expression = Expression.Call(typeof(Queryable), dynamicOrdering.Ascending ? text : text2, new Type[]
                {
                    source.ElementType,
                    dynamicOrdering.Selector.Type
                }, new Expression[]
                {
                    expression,
                    Expression.Quote(Expression.Lambda(dynamicOrdering.Selector, parameters))
                });
                text = "ThenBy";
                text2 = "ThenByDescending";
            }
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                expression
            });
        }

        public static IQueryable Take(this IQueryable source, string count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Take", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Constant(int.Parse(count))
                })
            });
        }

        public static IQueryable Skip(this IQueryable source, string count)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Skip", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Constant(int.Parse(count))
                })
            });
        }

        public static IQueryable GroupBy(this IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                typeof(IGrouping<, >).MakeGenericType(new Type[]
                {
                    lambdaExpression.Body.Type,
                    source.ElementType
                })
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "GroupBy", new Type[]
                {
                    source.ElementType,
                    lambdaExpression.Body.Type
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression)
                })
            });
        }

        public static IQueryable GroupBy(this IQueryable source, string keySelector, string elementSelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            if (elementSelector == null)
            {
                throw new ArgumentNullException("elementSelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            LambdaExpression lambdaExpression2 = DynamicExpression.ParseLambda(source.ElementType, null, elementSelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression2.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "GroupBy", new Type[]
                {
                    source.ElementType,
                    lambdaExpression.Body.Type,
                    lambdaExpression2.Body.Type
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression),
                    Expression.Quote(lambdaExpression2)
                })
            });
        }

        public static object Average(this IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression expression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            return source.Provider.Execute(Expression.Call(typeof(Queryable), "Average", new Type[]
            {
                source.ElementType
            }, new Expression[]
            {
                source.Expression,
                Expression.Quote(expression)
            }));
        }

        public static IQueryable CreateQueryAverage(IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Average", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression)
                })
            });
        }

        public static object Sum(this IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression expression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            return source.Provider.Execute(Expression.Call(typeof(Queryable), "Sum", new Type[]
            {
                source.ElementType
            }, new Expression[]
            {
                source.Expression,
                Expression.Quote(expression)
            }));
        }

        public static IQueryable CreateQuerySum(IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Sum", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression)
                })
            });
        }

        public static object Max(this IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            return source.Provider.Execute(Expression.Call(typeof(Queryable), "Max", new Type[]
            {
                source.ElementType,
                lambdaExpression.Body.Type
            }, new Expression[]
            {
                source.Expression,
                Expression.Quote(lambdaExpression)
            }));
        }

        public static IQueryable CreateQueryMax(IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Max", new Type[]
                {
                    source.ElementType,
                    lambdaExpression.Body.Type
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression)
                })
            });
        }

        public static object Min(this IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            return source.Provider.Execute(Expression.Call(typeof(Queryable), "Min", new Type[]
            {
                source.ElementType,
                lambdaExpression.Body.Type
            }, new Expression[]
            {
                source.Expression,
                Expression.Quote(lambdaExpression)
            }));
        }

        public static IQueryable CreateQueryMin(IQueryable source, string keySelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, keySelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Min", new Type[]
                {
                    source.ElementType,
                    lambdaExpression.Body.Type
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression)
                })
            });
        }

        public static bool Any(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (bool)source.Provider.Execute(Expression.Call(typeof(Queryable), "Any", new Type[]
            {
                source.ElementType
            }, new Expression[]
            {
                source.Expression
            }));
        }

        public static int Count(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return (int)source.Provider.Execute(Expression.Call(typeof(Queryable), "Count", new Type[]
            {
                source.ElementType
            }, new Expression[]
            {
                source.Expression
            }));
        }

        public static IQueryable DistinctQ(this IQueryable source)
        {
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Distinct", new Type[]
                {
                    source.ElementType
                }, new Expression[]
                {
                    source.Expression
                })
            });
        }

        public static IEnumerable Paging(this IQueryable source, Expression keySelector, EnumOrder order, int pageIndex, int pageSize)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (keySelector == null)
            {
                throw new ArgumentNullException("keySelector");
            }
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            MethodInfo method = typeof(DynamicQueryable).GetMethod("Paging", new Type[]
            {
                typeof(IQueryable),
                typeof(Expression),
                typeof(EnumOrder),
                typeof(int),
                typeof(int)
            });
            IQueryable source2 = (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(null, method, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(keySelector),
                    Expression.Constant(order),
                    Expression.Constant(pageIndex),
                    Expression.Constant(pageSize)
                })
            });
            return source2.ToEnumerable();
        }

        public static IEnumerable Paging(this IQueryable source, Expression keySelector, EnumOrder order, int pageIndex, int pageSize, out int total)
        {
            total = source.Count();
            return source.Paging(keySelector, order, pageIndex, pageSize);
        }

        public static IEnumerable Paging(this IQueryable source, string orderField, EnumOrder order, int pageIndex, int pageSize, out int total)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (orderField == null)
            {
                throw new ArgumentNullException("orderField");
            }
            LambdaExpression keySelector = DynamicExpression.ParseLambda(source.ElementType, null, orderField, new object[0]);
            return source.Paging(keySelector, order, pageIndex, pageSize, out total);
        }

        public static IEnumerable Paging(this IQueryable source, string orderField, EnumOrder order, int pageIndex, int pageSize)
        {
            int num = 0;
            return source.Paging(orderField, order, pageIndex, pageSize, out num);
        }

        public static List<TSource> Paging<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, EnumOrder order, int pageIndex, int pageSize, out int total)
        {
            return source.Paging(keySelector, order, pageIndex, pageSize, out total) as List<TSource>;
        }

        public static List<TSource> Paging<TSource, TKey>(this IQueryable<TSource> source, Expression<Func<TSource, TKey>> keySelector, EnumOrder order, int pageIndex, int pageSize)
        {
            int num = 0;
            return source.Paging(keySelector, order, pageIndex, pageSize, out num);
        }

        public static IEnumerable ToEnumerable(this IQueryable source)
        {
            MethodInfo methodInfo = DynamicQueryable.miToList.MakeGenericMethod(new Type[]
            {
                source.ElementType
            });
            return (IEnumerable)methodInfo.Invoke(null, new object[]
            {
                source
            });
        }

        public static object FirstOrDefaultQ(this IQueryable source)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            return source.Provider.Execute(Expression.Call(typeof(Queryable), "FirstOrDefault", new Type[]
            {
                source.ElementType
            }, new Expression[]
            {
                source.Expression
            }));
        }

        public static IQueryable Join(this IQueryable outer, IEnumerable inner, string outerParameterName, string outerSelector, string innerParameterName, string innerSelector, string resultsSelector, params object[] values)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            if (outerParameterName == null)
            {
                throw new ArgumentNullException("outerParameterName");
            }
            if (innerParameterName == null)
            {
                throw new ArgumentNullException("innerParameterName");
            }
            if (outerSelector == null)
            {
                throw new ArgumentNullException("outerSelector");
            }
            if (innerSelector == null)
            {
                throw new ArgumentNullException("innerSelector");
            }
            if (resultsSelector == null)
            {
                throw new ArgumentNullException("resultsSelctor");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(outer.ElementType, null, outerParameterName, outerSelector, values);
            LambdaExpression expression = DynamicExpression.ParseLambda(inner.AsQueryable().ElementType, null, innerParameterName, innerSelector, values);
            ParameterExpression[] parameters = new ParameterExpression[]
            {
                Expression.Parameter(outer.ElementType, outerParameterName),
                Expression.Parameter(inner.AsQueryable().ElementType, innerParameterName)
            };
            LambdaExpression lambdaExpression2 = DynamicExpression.ParseLambda(parameters, null, resultsSelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression2.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(outer.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Join", new Type[]
                {
                    outer.ElementType,
                    inner.AsQueryable().ElementType,
                    lambdaExpression.Body.Type,
                    lambdaExpression2.Body.Type
                }, new Expression[]
                {
                    outer.Expression,
                    inner.AsQueryable().Expression,
                    Expression.Quote(lambdaExpression),
                    Expression.Quote(expression),
                    Expression.Quote(lambdaExpression2)
                })
            });
        }

        public static IQueryable<T> Join<T>(this IQueryable<T> outer, IEnumerable<T> inner, string outerParameterName, string outerSelector, string innerParameterName, string innerSelector, string resultsSelector, params object[] values)
        {
            return (IQueryable<T>)outer.Join(inner, outerParameterName, outerSelector, innerParameterName, innerSelector, resultsSelector, values);
        }

        public static IQueryable Join(this IQueryable outer, IEnumerable inner, string outerSelector, string innerSelector, string resultsSelector, params object[] values)
        {
            if (inner == null)
            {
                throw new ArgumentNullException("inner");
            }
            if (outerSelector == null)
            {
                throw new ArgumentNullException("outerSelector");
            }
            if (innerSelector == null)
            {
                throw new ArgumentNullException("innerSelector");
            }
            if (resultsSelector == null)
            {
                throw new ArgumentNullException("resultsSelctor");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(outer.ElementType, null, outerSelector, values);
            LambdaExpression expression = DynamicExpression.ParseLambda(inner.AsQueryable().ElementType, null, innerSelector, values);
            ParameterExpression[] parameters = new ParameterExpression[]
            {
                Expression.Parameter(outer.ElementType, "outer"),
                Expression.Parameter(inner.AsQueryable().ElementType, "inner")
            };
            LambdaExpression lambdaExpression2 = DynamicExpression.ParseLambda(parameters, null, resultsSelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression2.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(outer.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "Join", new Type[]
                {
                    outer.ElementType,
                    inner.AsQueryable().ElementType,
                    lambdaExpression.Body.Type,
                    lambdaExpression2.Body.Type
                }, new Expression[]
                {
                    outer.Expression,
                    inner.AsQueryable().Expression,
                    Expression.Quote(lambdaExpression),
                    Expression.Quote(expression),
                    Expression.Quote(lambdaExpression2)
                })
            });
        }

        public static IQueryable<T> Join<T>(this IQueryable<T> outer, IEnumerable<T> inner, string outerSelector, string innerSelector, string resultsSelector, params object[] values)
        {
            return (IQueryable<T>)outer.Join(inner, outerSelector, innerSelector, resultsSelector, values);
        }

        public static IQueryable SelectMany(this IQueryable source, string selector, string resultsSelector, params object[] values)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }
            LambdaExpression lambdaExpression = DynamicExpression.ParseLambda(source.ElementType, null, selector, values);
            Type type = source.Expression.Type.GetGenericArguments()[0];
            Type type2 = lambdaExpression.Body.Type.GetGenericArguments()[0];
            Type type3 = typeof(IEnumerable<>).MakeGenericType(new Type[]
            {
                type2
            });
            Type delegateType = typeof(Func<,>).MakeGenericType(new Type[]
            {
                type,
                type3
            });
            lambdaExpression = Expression.Lambda(delegateType, lambdaExpression.Body, lambdaExpression.Parameters);
            ParameterExpression[] parameters = new ParameterExpression[]
            {
                Expression.Parameter(source.ElementType, "outer"),
                Expression.Parameter(type2, "inner")
            };
            LambdaExpression lambdaExpression2 = DynamicExpression.ParseLambda(parameters, null, resultsSelector, values);
            MethodInfo methodInfo = DynamicQueryable.miCreateQuery.MakeGenericMethod(new Type[]
            {
                lambdaExpression2.Body.Type
            });
            return (IQueryable)methodInfo.Invoke(source.Provider, new object[]
            {
                Expression.Call(typeof(Queryable), "SelectMany", new Type[]
                {
                    source.ElementType,
                    type2,
                    lambdaExpression2.Body.Type
                }, new Expression[]
                {
                    source.Expression,
                    Expression.Quote(lambdaExpression),
                    Expression.Quote(lambdaExpression2)
                })
            });
        }

        public static IQueryable<T> SelectMany<T>(this IQueryable<T> source, string selector, string resultsSelector, params object[] values)
        {
            return (IQueryable<T>)source.SelectMany(selector, resultsSelector, values);
        }

        private static MethodInfo miCreateQuery;

        private static MethodInfo miToList;
    }
}
