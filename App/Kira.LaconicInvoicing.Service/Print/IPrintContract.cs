using Kira.LaconicInvoicing.Print.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.Print
{
    public interface IPrintContract
    {
        /// <summary>
        /// 保存临时模板文件
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        Task<string> TemplateSaveAsync(IFormFile file);

        /// <summary>
        /// 添加模板
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task AddAsync(PrintTemplateInputDto dto, IFormFile file = null);

        /// <summary>
        /// 更新模板信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task UpdateAsync(PrintTemplateInputDto dto, IFormFile file = null);

        /// <summary>
        /// 删除模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// 获取指定模板
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<PrintTemplateOutputDto> GetAsync(Guid id);

        /// <summary>
        /// 获取所有模板
        /// </summary>
        /// <returns></returns>
        Task<List<PrintTemplateOutputDto>> GetAllAsync();

        /// <summary>
        /// 获取指定模板类型的所有模板
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<PrintTemplateOutputDto>> GetAllByTypeAsync(TemplateType type);

        /// <summary>
        /// 获取模板文件内容
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        Task<string> GetPrintTemplateScriptAsync(string path);
    }
}