using Kira.LaconicInvoicing.Common.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.BaseData
{
    public interface IBaseDataContract
    {
        /// <summary>
        /// 获取或刷新数据字典值
        /// </summary>
        /// <param name="type">数据字典key</param>
        /// <returns>数据字典值</returns>
        Task<Kira.LaconicInvoicing.Common.Entities.BaseData> GetOrRefreshAsync(string type);
        /// <summary>
        /// 获取基础数据列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<Dictionary<string, string>> GetListAsync(string type);

        /// <summary>
        /// 更新基础数据列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> UpdateListAsync(string type, BaseDataListDto dto);

        /// <summary>
        /// 添加基础数据列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<bool> AddListAsync(string type, BaseDataListDto dto);

        /// <summary>
        /// 删除基础数据列表
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<bool> DeleteListAsync(string type, string name);

        /// <summary>
        /// 更新基础数据值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> UpdateValueAsync(string type, string value);

        /// <summary>
        /// 获取基础数据值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<string> GetValueAsync(string type);
    }
}
