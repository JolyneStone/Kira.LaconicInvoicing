using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public interface IPurchaseOrderContract
    {
        /// <summary>
        /// 获取新的采购单编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询采购单
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PageData<PurchaseOrderOutputDto>> SearchAsync(QueryCondition<PurchaseOrder> query = null);

        /// <summary>
        /// 添加采购单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddPurchaseOrderAsync(PurchaseOrderInputDto dto);

        /// <summary>
        /// 根据指定Id获取采购单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PurchaseOrderOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新采购单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdatePurchaseOrderAsync(PurchaseOrderInputDto dto);

        /// <summary>
        /// 批量删除采购单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
