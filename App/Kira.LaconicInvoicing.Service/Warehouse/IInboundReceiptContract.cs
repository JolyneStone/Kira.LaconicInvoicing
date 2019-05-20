using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public interface IInboundReceiptContract
    {
        /// <summary>
        /// 获取新的入库单编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询入库单
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<PageData<InboundReceiptOutputDto>> SearchAsync(QueryCondition<InboundReceipt> condition = null);

        /// <summary>
        /// 添加入库单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddInboundReceiptAsync(InboundReceiptInputDto dto, IServiceProvider serviceProvider);

        /// <summary>
        /// 根据指定Id获取入库单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<InboundReceiptOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新入库单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateInboundReceiptAsync(InboundReceiptInputDto dto);

        /// <summary>
        /// 批量删除入库单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
