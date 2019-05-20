using Kira.LaconicInvoicing.File.Dtos;
using Kira.LaconicInvoicing.Service.File;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.File.Controllers
{
    [ModuleInfo(Order = 1, Position = "File", PositionName = "文档模块")]
    [Description("管理-文档模板")]
    public class FileManagementController : FileApiController
    {
        private readonly IFileContract _fileContract;

        /// <summary>
        /// 初始化一个<see cref="FileManagementController"/>类型的新实例
        /// </summary>
        public FileManagementController(IFileContract fileContract)
        {
            _fileContract = fileContract;
        }


        /// <summary>
        /// 保存文档模板文件信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("保存文档模板文件信息")]
        public async Task<AjaxResult> UploadTemplate(IFormFile file)
        {
            return await AjaxResult.Business(async result =>
            {
                file = file ?? (Request.Form.Files.Count > 0 ? Request.Form.Files[0] : null);
                Check.NotNull(file, nameof(file));
                var path = await _fileContract.TemplateSaveAsync(file);
                result.Success(path);
            });
        }

        /// <summary>
        /// 添加文档模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("添加文档模板信息")]
        public async Task<AjaxResult> Add(FileTemplateInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                dto.Id = Guid.NewGuid();
                dto.DateTime = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                await _fileContract.AddAsync(dto);
                result.Success();
            });
        }

        /// <summary>
        /// 更新文档模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新文档模板信息")]
        public async Task<AjaxResult> Update(FileTemplateInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                dto.DateTime = DateTime.Now;
                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }

                await _fileContract.UpdateAsync(dto);
                result.Success();
            });
        }

        /// <summary>
        /// 删除文档模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除文档模板信息")]
        public async Task<AjaxResult> Delete(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                await _fileContract.DeleteAsync(id);
                result.Success();
            });
        }

        /// <summary>
        /// 获取所有文档模板信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取所有文档模板信息")]
        public async Task<AjaxResult> GetAll()
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _fileContract.GetAllAsync());
            });
        }

        /// <summary>
        /// 下载文档模板文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [AllowAnonymous]
        [Description("下载文档模板文件")]
        public IActionResult Download(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));
            var fullPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}{path}";
            var file = new FileInfo(fullPath);
            if (!file.Exists)
                throw new BussinessException("无法找到指定套打模板文件");

            var stream = file.OpenRead();
            var fileExtend = path.Substring(path.LastIndexOf('.'));
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExtend];
            return File(stream, memi, Path.GetFileName(fullPath));
        }
    }
}
