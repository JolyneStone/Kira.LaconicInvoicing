using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using OSharp.Filter;
using System;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public interface IProductContract
    {
        /// <summary>
        /// 获取新的产品编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询产品
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PageData<ProductOutputDto>> SearchAsync(QueryCondition<Product> query = null);

        /// <summary>
        /// 添加产品
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddProductAsync(ProductInputDto dto);

        /// <summary>
        /// 根据指定Id获取产品
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProductOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新产品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateProductAsync(ProductInputDto dto);

        /// <summary>
        /// 批量删除产品
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);

        /// <summary>
        /// 获取指定产品的供应商集合
        /// </summary>
        /// <param name="query"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<PageData<CustomerOutputDto>> GetCustomers(QueryCondition<CustomerProduct> query, IServiceProvider serviceProvider);

        /// <summary>
        /// 获取更新指定产品的供应商集合
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ProductIds"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<bool> UpdateCustomers(Guid id, Guid[] customerIds, IServiceProvider serviceProvider);
    }
}
