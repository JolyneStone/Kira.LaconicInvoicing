using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public interface IInventoryContract
    {
        /// <summary>
        /// 查询库存
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<PageData<InventoryOutputDto>> SearchAsync(QueryCondition<Inventory> condition = null);

        /// <summary>
        /// 查询原料库存
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<PageData<InventoryOutputDto>> SearchMaterialAsync(QueryCondition<Inventory> condition, IServiceProvider serviceProvider);

        /// <summary>
        /// 查询产品库存
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<PageData<InventoryOutputDto>> SearchProductAsync(QueryCondition<Inventory> condition, IServiceProvider serviceProvider);

        /// <summary>
        /// 添加库存
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddInventoryAsync(InventoryInputDto dto);

        /// <summary>
        /// 根据指定Id获取库存
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InventoryOutputDto> GetAsync(Guid id, IServiceProvider serviceProvider);

        /// <summary>
        /// 更新库存信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateInventoryAsync(InventoryInputDto dto);

        /// <summary>
        /// 批量删除库存
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
