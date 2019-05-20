using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public interface IOutboundReceiptContract
    {
        /// <summary>
        /// 获取新的出库单编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询出库单
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<PageData<OutboundReceiptOutputDto>> SearchAsync(QueryCondition<OutboundReceipt> condition = null);

        /// <summary>
        /// 添加出库单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddOutboundReceiptAsync(OutboundReceiptInputDto dto, IServiceProvider serviceProvider);

        /// <summary>
        /// 根据指定Id获取出库单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<OutboundReceiptOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新出库单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateOutboundReceiptAsync(OutboundReceiptInputDto dto);

        /// <summary>
        /// 批量删除出库单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
