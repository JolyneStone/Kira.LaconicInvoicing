using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public interface IMaterialContract
    {
        /// <summary>
        /// 获取新的原料编号
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<string> GetNewNumber(IServiceProvider serviceProvider);

        /// <summary>
        /// 查询原料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        Task<PageData<MaterialOutputDto>> SearchAsync(QueryCondition<Material> query = null);

        /// <summary>
        /// 添加原料
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddMaterialAsync(MaterialInputDto dto);

        /// <summary>
        /// 根据指定Id获取原料
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<MaterialOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 更新原料信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateMaterialAsync(MaterialInputDto dto);

        /// <summary>
        /// 批量删除原料
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        Task<bool> DeleteAsync(Guid[] ids);

        /// <summary>
        /// 获取指定原料的供应商集合
        /// </summary>
        /// <param name="query"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<PageData<VendorOutputDto>> GetVendors(QueryCondition<VendorMaterial> query, IServiceProvider serviceProvider);

        /// <summary>
        /// 获取更新指定原料的供应商集合
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vendorIds"></param>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task<bool> UpdateVendors(Guid id, Guid[] vendorIds, IServiceProvider serviceProvider);
    }
}
