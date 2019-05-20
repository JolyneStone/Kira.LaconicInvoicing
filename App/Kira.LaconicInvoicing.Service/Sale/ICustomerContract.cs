using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public interface ICustomerContract
    {
        /// <summary>
        /// 获取新的客户编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询客户
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<PageData<CustomerOutputDto>> SearchAsync(QueryCondition<Customer> condition = null);

        /// <summary>
        /// 添加客户
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddCustomerAsync(CustomerInputDto dto);

        /// <summary>
        /// 根据指定Id获取客户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CustomerOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateCustomerAsync(CustomerInputDto dto);

        /// <summary>
        /// 批量删除客户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);

        /// <summary>
        /// 获取指定客户的产品集合
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<PageData<ProductOutputDto>> GetProducts(QueryCondition<CustomerProduct> queryCondition, IServiceProvider serviceProvider);

        /// <summary>
        /// 获取更新指定客户的产品集合
        /// </summary>
        /// <param name="id"></param>
        /// <param name="materialIds"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<bool> UpdateProducts(Guid id, Guid[] productIds, IServiceProvider serviceProvider);
    }
}
