using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kira.LaconicInvoicing
{
    public static class QueryableExtension
    {
        public static IQueryable<T> FilterDateTime<T>(this IQueryable<T> @this, ref QueryCondition<T> condition) where T : IDateTimeEntity
        {
            if (@this == null || condition == null || condition.Filters == null)
                return null;

            for (var i = 0; i < condition.Filters.Count; i++)
            {
                var filter = condition.Filters[i];
                if (!string.IsNullOrWhiteSpace(filter.Field) &&
                    filter.Value != null &&
                    filter.Field.EqualsIgnoreCase("DateTime"))
                {
                    if (filter.Value == null)
                    {
                        @this = @this.Where(q => q.DateTime == null);
                    }
                    else
                    {
                        var dateTime = DateTime.Parse(filter.Value.ToString());
                        switch (filter.ConditionOp)
                        {
                            case ConditionOperator.Equal:
                                @this = @this.Where(q => q.DateTime == dateTime);
                                break;
                            case ConditionOperator.GreaterThan:
                                @this = @this.Where(q => q.DateTime > dateTime);
                                break;
                            case ConditionOperator.GreaterThanOrEqual:
                                @this = @this.Where(q => q.DateTime >= dateTime);
                                break;
                            case ConditionOperator.NotEqual:
                                @this = @this.Where(q => q.DateTime != dateTime);
                                break;
                            case ConditionOperator.LessThan:
                                @this = @this.Where(q => q.DateTime < dateTime);
                                break;
                            case ConditionOperator.LessThanOrEqual:
                                @this = @this.Where(q => q.DateTime <= dateTime);
                                break;
                            default:
                                break;
                        }
                    }

                    condition.Filters[i] = null;
                }
            }

            return @this;
        }
    }
}
