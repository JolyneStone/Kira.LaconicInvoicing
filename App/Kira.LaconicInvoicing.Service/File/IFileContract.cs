using Kira.LaconicInvoicing.File.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Service.File
{
    public interface IFileContract
    {
        Task<string> TemplateSaveAsync(IFormFile file);

        Task AddAsync(FileTemplateInputDto dto);

        Task UpdateAsync(FileTemplateInputDto dto);

        Task DeleteAsync(Guid id);

        Task<List<FileTemplateOutputDto>> GetAllAsync();

        Task<FileTemplateOutputDto> GetAsync(Guid id);

        Task<List<FileTemplateOutputDto>> GetAllByTypeAsync(TemplateType type);

        Task<ValueTuple<string, string>> ExportAsync(IServiceProvider serviceProvider, Guid id, Guid templateId, TemplateType type);
    }
}