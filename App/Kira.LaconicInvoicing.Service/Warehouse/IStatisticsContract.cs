using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public interface IStatisticsContract
    {
        /// <summary>
        /// 库存统计
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ValueTuple<PieChartDto, PieChartDto, PieChartDto>> StatisticsInventoryAnalysisAsync(
            IServiceProvider serviceProvider,
            Guid? id = null);

        /// <summary>
        /// 获取仓库列表
        /// </summary>
        /// <returns></returns>
        Task<IList<SimpleWarehouseDto>> GetWarehousesAsync();
    }
}
