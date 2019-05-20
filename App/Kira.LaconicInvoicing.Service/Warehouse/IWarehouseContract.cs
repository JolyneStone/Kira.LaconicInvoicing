using Kira.LaconicInvoicing.Warehouse.Dtos;
using OSharp.Filter;
using System;
using System.Threading.Tasks;
using WarehouseEntity = Kira.LaconicInvoicing.Warehouse.Entities.Warehouse;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public interface IWarehouseContract
    {
        /// <summary>
        /// 获取新的仓库编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询仓库
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PageData<WarehouseOutputDto>> SearchAsync(QueryCondition<WarehouseEntity> query = null);

        /// <summary>
        /// 添加仓库
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddWarehouseAsync(WarehouseInputDto dto);

        /// <summary>
        /// 根据指定Id获取仓库
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<WarehouseOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新仓库信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateWarehouseAsync(WarehouseInputDto dto);

        /// <summary>
        /// 批量删除仓库
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
