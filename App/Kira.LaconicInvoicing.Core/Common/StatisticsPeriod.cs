using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Common
{
    public enum StatisticsPeriod
    {
        [EnumDisplayName("按月份统计")]
        Month = 0,
        [EnumDisplayName("按年份统计")]
        Year =1
    }
}
