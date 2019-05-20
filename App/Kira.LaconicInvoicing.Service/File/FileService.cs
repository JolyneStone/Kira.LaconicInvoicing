using Kira.LaconicInvoicing.File.Dtos;
using Kira.LaconicInvoicing.File.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Linq;
using Kira.LaconicInvoicing.Service.Sale;
using Kira.LaconicInvoicing.Service.Warehouse;
using Kira.LaconicInvoicing.Service.Purcachase;
using System.Reflection;
using Kira.LaconicInvoicing.Infrastructure.Word;
using System.Collections;

namespace Kira.LaconicInvoicing.Service.File
{
    public class FileService : IFileContract
    {
        private readonly IRepository<FileTemplate, Guid> _printRepo;
        private readonly ILogger<FileService> _logger;
        private readonly string _templatesPath;

        public FileService(IRepository<FileTemplate, Guid> printRepo,
            ILogger<FileService> logger)
        {
            _printRepo = printRepo;
            _logger = logger;
            _templatesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates");
        }

        public async Task<string> TemplateSaveAsync(IFormFile file)
        {
            var directoryInfo = new DirectoryInfo(_templatesPath);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            //var fileExtend = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            //var fileName = Guid.NewGuid().ToString() + fileExtend;
            var fileName = Guid.NewGuid().ToString() + ".dot";
            var fileFullName = $"{_templatesPath}{Path.DirectorySeparatorChar}{fileName}";
            using (var stream = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task AddAsync(FileTemplateInputDto dto)
        {
            await _printRepo.InsertAsync(dto.MapTo<FileTemplate>());
        }

        public async Task UpdateAsync(FileTemplateInputDto dto)
        {
            var fileTemplate = await _printRepo.GetAsync(dto.Id);
            var fileInfo = new FileInfo(Path.Combine(_templatesPath, fileTemplate.Path));
            if (fileInfo.Exists)
            {
                fileInfo.Delete();
            }
            await _printRepo.UpdateAsync(dto.MapTo<FileTemplate>());
        }

        public async Task DeleteAsync(Guid id)
        {
            var printTemplate = await _printRepo.GetAsync(id);
            if (printTemplate != null)
            {
                var fileInfo = new FileInfo($"{_templatesPath}{Path.DirectorySeparatorChar}{printTemplate.Path}");
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                await _printRepo.DeleteAsync(printTemplate);
            }
        }

        public async Task<List<FileTemplateOutputDto>> GetAllAsync()
        {
            return await _printRepo.Query().Select(p => p.MapTo<FileTemplateOutputDto>()).ToListAsync();
        }

        public async Task<FileTemplateOutputDto> GetAsync(Guid id)
        {
            return (await _printRepo.GetAsync(id))?.MapTo<FileTemplateOutputDto>();
        }

        public async Task<List<FileTemplateOutputDto>> GetAllByTypeAsync(TemplateType type)
        {
            return await _printRepo.Query().Where(p => p.Type == type).Select(p => p.MapTo<FileTemplateOutputDto>()).ToListAsync();
        }

        public async Task<ValueTuple<string, string>> ExportAsync(IServiceProvider serviceProvider, Guid id, Guid templateId, TemplateType type)
        {
            var fileTemplate = await GetAsync(templateId);
            if (fileTemplate == null)
                throw new BussinessException("无法找到指定的文档模板");

            object data = null;
            switch (type)
            {
                case TemplateType.PurchaseOrder:
                    var purchaseOrderContract = serviceProvider.GetService<IPurchaseOrderContract>();
                    data = await purchaseOrderContract.GetAsync(id);
                    break;
                case TemplateType.InboundReceipt:
                    var inboundReceiptContract = serviceProvider.GetService<IInboundReceiptContract>();
                    data = await inboundReceiptContract.GetAsync(id);
                    break;
                case TemplateType.OutboundReceipt:
                    var outboundReceiptContract = serviceProvider.GetService<IOutboundReceiptContract>();
                    data = await outboundReceiptContract.GetAsync(id);
                    break;
                case TemplateType.TransferOrder:
                    var transferOrderContract = serviceProvider.GetService<ITransferOrderContract>();
                    data = await transferOrderContract.GetAsync(id);
                    break;
                case TemplateType.SaleOrder:
                    var saleOrderContract = serviceProvider.GetService<ISaleOrderContract>();
                    data = await saleOrderContract.GetAsync(id);
                    break;
                default:
                    throw new BussinessException("模板类型不正确");
            }

            if (data == null)
            {
                throw new BussinessException("找不到指定表单数据");
            }

            var fullPath = Path.Combine(_templatesPath, fileTemplate.Path);
            var word = new ExportWord(fullPath);
            word.OnExport = () =>
            {
                SetBookMark(word, data, "");
            };

            var tempDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
            var directory = new DirectoryInfo(tempDirectory);
            if (!directory.Exists)
                directory.Create();
            try
            {
                foreach (var file in directory.GetFiles())
                {
                    file.Delete();
                }
            }
            catch
            {
            }

            var fileName = Guid.NewGuid().ToString();
            var tragetPath = Path.Combine(tempDirectory, fileName + ".docx");
            word.Export(tragetPath);
            return (type.GetDisplayName(), fileName);
        }

        private void SetBookMark(ExportWord exportWord, object data, string prefix = null)
        {
            if (data == null)
                return;

            foreach (var property in data.GetType().GetProperties(
                BindingFlags.Public |
                BindingFlags.Instance |
                BindingFlags.GetProperty))
            {
                var propertyValue = property.GetValue(data);
                if (propertyValue == null)
                    continue;

                var propertyType = property.PropertyType;
                var bookMarkName = prefix + property.Name;
                if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }
                if (propertyType == typeof(double))
                {
                    exportWord.SetBookMarkText(bookMarkName, Math.Round((double)propertyValue).ToString());
                }
                else if (propertyType == typeof(float) || property == typeof(float?))
                {
                    exportWord.SetBookMarkText(bookMarkName, Math.Round((double)(float)propertyValue).ToString());
                }
                else if (propertyType == typeof(decimal))
                {
                    exportWord.SetBookMarkText(bookMarkName, Math.Round((decimal)propertyValue).ToString());
                }
                else if (propertyType.BaseType == typeof(Enum))
                {
                    exportWord.SetBookMarkText(bookMarkName, ((Enum)propertyValue).GetDisplayName());
                }
                else if (propertyType == typeof(DateTime))
                {
                    exportWord.SetBookMarkText(bookMarkName, ((DateTime)propertyValue).ToString("yyyy年MM月dd日"));
                }
                else if (propertyType != typeof(string) && (typeof(IEnumerable)).IsAssignableFrom(propertyType))
                {
                    var i = 0;
                    foreach (var item in (IEnumerable)propertyValue)
                    {
                        SetBookMark(exportWord, item, bookMarkName + i.ToString());
                    }
                }
                else
                {
                    exportWord.SetBookMarkText(bookMarkName, propertyValue.ToString());
                }
            }
        }
    }
}