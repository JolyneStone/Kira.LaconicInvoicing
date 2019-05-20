using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public interface ITransferOrderContract
    {
        /// <summary>
        /// 获取新的调拨单单编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询调拨单单
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<PageData<TransferOrderOutputDto>> SearchAsync(QueryCondition<TransferOrder> condition = null);

        /// <summary>
        /// 添加调拨单单
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddTransferOrderAsync(TransferOrderInputDto dto, IServiceProvider serviceProvider);

        /// <summary>
        /// 根据指定Id获取调拨单单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TransferOrderOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新调拨单单信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateTransferOrderAsync(TransferOrderInputDto dto);

        /// <summary>
        /// 批量删除调拨单单
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);
    }
}
