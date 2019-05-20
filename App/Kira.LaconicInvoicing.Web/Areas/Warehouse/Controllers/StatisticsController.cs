using Kira.LaconicInvoicing.Service.Warehouse;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Warehouse.Controllers
{
    [ModuleInfo(Order = 1, Position = "Warehouse", PositionName = "仓库模块")]
    [Description("管理-仓库统计分析")]
    public class StatisticsController : WarehouseApiController
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
        /// 获取库存统计分析
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取库存统计分析")]
        public async Task<AjaxResult> StatisticsInventoryAnalysis(Guid? id = null)
        {
            return await AjaxResult.Business(async result =>
            {
                var (warehousePie, materialPie, productPie) = await _statisticsContract.StatisticsInventoryAnalysisAsync(ServiceProvider, id);
                result.Success(new { warehousePie, materialPie, productPie });
            });
        }

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("获取仓库列表")]
        public async Task<AjaxResult> GetWarehouses()
        {
            return await AjaxResult.Business(async result =>
            {
                var data = await _statisticsContract.GetWarehousesAsync();
                result.Success(data);
            });
        }
    }
}
