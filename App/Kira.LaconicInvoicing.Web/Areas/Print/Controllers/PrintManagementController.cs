using Kira.LaconicInvoicing.Infrastructure.Options;
using Kira.LaconicInvoicing.Print.Dtos;
using Kira.LaconicInvoicing.Service.Print;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System;
using System.ComponentModel;
using System.IO;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Print.Controllers
{
    [ModuleInfo(Order = 1, Position = "Print", PositionName = "套打模块")]
    [Description("管理-套打模板")]
    public class PrintManagementController : PrintApiController
    {
        private readonly IPrintContract _printContract;

        /// <summary>
        /// 初始化一个<see cref="PrintManagementController"/>类型的新实例
        /// </summary>
        public PrintManagementController(IPrintContract printContract)
        {
            _printContract = printContract;
        }

        ///// <summary>
        ///// 保存套打模板文件信息
        ///// </summary>
        ///// <returns></returns>
        //[HttpPost]
        //[ModuleInfo]
        //[Description("保存套打模板文件信息")]
        //public async Task<AjaxResult> UploadTemplate(IFormFile file)
        //{
        //    return await AjaxResult.Business(async result =>
        //    {
        //        Check.NotNull(file, nameof(file));
        //        var path = await _printContract.TemplateSaveAsync(file);
        //        result.Success(path);
        //    });
        //}

        /// <summary>
        /// 添加套打模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("添加套打模板信息")]
        public async Task<AjaxResult> Add(PrintTemplateInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                dto.Id = Guid.NewGuid();
                dto.DateTime = DateTime.Now;
                if (!ModelState.IsValid )
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                await _printContract.AddAsync(dto);
                result.Success();
            });
        }

        /// <summary>
        /// 更新套打模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新套打模板信息")]
        public async Task<AjaxResult> Update(PrintTemplateInputDto dto)
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

                await _printContract.UpdateAsync(dto);
                result.Success();
            });
        }

        /// <summary>
        /// 删除套打模板信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除套打模板信息")]
        public async Task<AjaxResult> Delete(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                await _printContract.DeleteAsync(id);
                result.Success();
            });
        }

        /// <summary>
        /// 获取指定模板信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取指定模板信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                result.Success(await _printContract.GetAsync(id));
            });
        }

        /// <summary>
        /// 获取所有套打模板信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取所有套打模板信息")]
        public async Task<AjaxResult> GetAll()
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _printContract.GetAllAsync());
            });
        }

        /// <summary>
        /// 获取套打模板代码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取套打模板代码")]
        public async Task<AjaxResult> GetPrintTemplateScript(string path)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNullOrEmpty(path, nameof(path));
                result.Success(await _printContract.GetPrintTemplateScriptAsync(path));
            });
        }

        /// <summary>
        /// 下载套打模板文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [AllowAnonymous]
        [Description("下载套打模板文件")]
        public IActionResult Download(string path)
        {
            Check.NotNullOrEmpty(path, nameof(path));
            var fullPath = $"{Directory.GetCurrentDirectory()}{Path.DirectorySeparatorChar}wwwroot{Path.DirectorySeparatorChar}templates{Path.DirectorySeparatorChar}{path}";
            var file = new FileInfo(fullPath);
            if (!file.Exists)
                throw new BussinessException("无法找到指定打印模板文件");

            var stream = file.OpenRead();
            var fileExtend = path.Substring(path.LastIndexOf('.'));
            //获取文件的ContentType
            var provider = new FileExtensionContentTypeProvider();
            var memi = provider.Mappings[fileExtend];
            return File(stream, memi, Path.GetFileName(fullPath));
        }
    }
}
