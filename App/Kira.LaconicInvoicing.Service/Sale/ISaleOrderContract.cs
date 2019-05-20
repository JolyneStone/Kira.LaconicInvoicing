using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public interface ISaleOrderContract
    {
        /// <summary>
        /// 获取新的销售单编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询销售单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PageData<SaleOrderOutputDto>> SearchAsync(QueryCondition<SaleOrder> query = null);

        /// <summary>
        /// 添加销售单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddSaleOrderAsync(SaleOrderInputDto dto);

        /// <summary>
        /// 根据指定Id获取销售单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SaleOrderOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新销售单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateSaleOrderAsync(SaleOrderInputDto dto);

        /// <summary>
        /// 批量删除销售单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
