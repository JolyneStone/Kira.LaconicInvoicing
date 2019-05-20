using Kira.LaconicInvoicing.Common;
using Kira.LaconicInvoicing.Common.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public interface IStatisticsContract
    {
        /// <summary>
        /// 趋势统计分析
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="period"></param>
        /// <returns></returns>
        Task<ValueTuple<ColumnChartDto, ColumnChartDto>> TrendStatisticsAnalysisAsync(DateTime startDate, DateTime endDate, StatisticsPeriod period);

        /// <summary>
        /// 总体统计分析
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        Task<ValueTuple<PieChartDto, PieChartDto, PieChartDto, PieChartDto>> GeneralStatisticsAnalysisAsync(IServiceProvider serviceProvider, DateTime startDate, DateTime endDate);
    }
}
