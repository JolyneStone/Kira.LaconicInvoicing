using Kira.LaconicInvoicing.Common;
using Kira.LaconicInvoicing.Service.Purcachase;
using Kira.LaconicInvoicing.Web.Areas.Purchase.Controllers;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Purcashase.Controllers
{
    [ModuleInfo(Order = 1, Position = "Purchase", PositionName = "采购模块")]
    [Description("管理-采购统计分析")]
    public class StatisticsController : PurchaseApiController
    {
        private readonly IStatisticsContract _statisticsContract;

        /// <summary>
        /// 初始化一个<see cref="StatisticsController"/>类型的新实例
        /// </summary>
        public StatisticsController(IStatisticsContract statisticsContract)
        {
            _statisticsContract = statisticsContract;
        }

        /// <summary>
        /// 获取采购趋势统计分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取采购趋势统计分析")]
        public async Task<AjaxResult> TrendStatisticsAnalysis(DateTime startDate, DateTime endDate, StatisticsPeriod period)
        {
            return await AjaxResult.Business(async result =>
            {
                if (startDate >= endDate)
                {
                    throw new BussinessException("此时间段不受支持");
                }

                var (amountColumn, quantityColumn) = 
                    await _statisticsContract.TrendStatisticsAnalysisAsync(startDate, endDate, period);
                result.Success(new { amountColumn, quantityColumn });
            });
        }

        /// <summary>
        /// 获取采购整体分析数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取采购整体分析数据")]
        public async Task<AjaxResult> GeneralStatisticsAnalysis(DateTime startDate, DateTime endDate)
        {
            return await AjaxResult.Business(async result =>
            {
                if (startDate >= endDate)
                {
                    throw new BussinessException("此时间段不受支持");
                }

                var (vendorAmountPie, vendorQuantityPie, materialAmountPie, materialQuantityPie) = 
                    await _statisticsContract.GeneralStatisticsAnalysisAsync(ServiceProvider, startDate, endDate);
                result.Success(new { vendorAmountPie, vendorQuantityPie, materialAmountPie, materialQuantityPie });
            });
        }
    }
}
