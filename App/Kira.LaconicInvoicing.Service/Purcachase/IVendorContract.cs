using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public interface IVendorContract
    {
        /// <summary>
        /// 获取新的供应商编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询供应商
        /// </summary>
        /// <param name="condition"></param>
        /// <returns></returns>
        Task<PageData<VendorOutputDto>> SearchAsync(QueryCondition<Vendor> condition = null);

        /// <summary>
        /// 添加供应商
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddVendorAsync(VendorInputDto dto);

        /// <summary>
        /// 根据指定Id获取供应商
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<VendorOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新供应商信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateVendorAsync(VendorInputDto dto);

        /// <summary>
        /// 批量删除供应商
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);

        /// <summary>
        /// 获取指定供应商的原料集合
        /// </summary>
        /// <param name="queryCondition"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<PageData<MaterialOutputDto>> GetMaterials(QueryCondition<VendorMaterial> queryCondition, IServiceProvider serviceProvider);

        /// <summary>
        /// 获取更新指定供应商的原料集合
        /// </summary>
        /// <param name="id"></param>
        /// <param name="materialIds"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<bool> UpdateMaterials(Guid id, Guid[] materialIds, IServiceProvider serviceProvider);
    }
}
