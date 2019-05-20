using Kira.LaconicInvoicing.Infrastructure.Word;
using Kira.LaconicInvoicing.Service.File;
using Kira.LaconicInvoicing.Service.Purcachase;
using Kira.LaconicInvoicing.Service.Sale;
using Kira.LaconicInvoicing.Service.Warehouse;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System;
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.File.Controllers
{
    [ModuleInfo(Order = 1, Position = "File", PositionName = "文档模块")]
    [Description("文档-导出数据")]
    public class FileController : FileApiController
    {
        private readonly IFileContract _fileContract;

        /// <summary>
        /// 初始化一个<see cref="FileController"/>类型的新实例
        /// </summary>
        public FileController(IFileContract fileContract)
        {
            _fileContract = fileContract;
        }

        /// <summary>
        /// 获取指定类型所有文档模板信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取指定类型所有文档模板信息")]
        public async Task<AjaxResult> GetAllByType(TemplateType type)
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _fileContract.GetAllByTypeAsync(type));
            });
        }

        /// <summary>
        /// 导出文档
        /// </summary>
        /// <param name="id"></param>
        /// <param name="templateId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("导出文档")]
        public async Task<AjaxResult> ExportDocument(Guid id, Guid templateId, TemplateType type)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));
                Check.NotNull(templateId, nameof(templateId));
                Check.NotEmpty(templateId, nameof(templateId));

                var (fileName, destName) = await _fileContract.ExportAsync(ServiceProvider, id, templateId, type);
                result.Success(new { fileName, destName });
                //var file = new FileInfo(tragetPath);
                //var stream = file.OpenRead();
                //var fileExtend = ".docx";
                ////获取文件的ContentType
                //var provider = new FileExtensionContentTypeProvider();
                //var memi = provider.Mappings[fileExtend];
                //return File(stream, memi, fileName);
            });
        }

        /// <summary>
        /// 下载文档
        /// </summary>
        /// <param name="name"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Description("下载文档")]
        public IActionResult Download(string name, string fileName)
        {
            Check.NotNullOrEmpty(name, nameof(name));
            var fileExtend = ".docx";
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp", name + ".docx");
            var file = new FileInfo(fullPath);
            if (!file.Exists)
                throw new BussinessException("无法找到指定文档");

            var stream = file.OpenRead();
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExtend];
            return File(stream, memi, fileName + fileExtend);
        }
    }
}
