using Kira.LaconicInvoicing.Service.Print;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Print.Controllers
{
    [ModuleInfo(Order = 1, Position = "Print", PositionName = "套打模块")]
    [Description("套打-打印数据")]
    public class PrintController : PrintApiController
    {
        private readonly IPrintContract _printContract;

        /// <summary>
        /// 初始化一个<see cref="PrintController"/>类型的新实例
        /// </summary>
        public PrintController(IPrintContract printContract)
        {
            _printContract = printContract;
        }

        /// <summary>
        /// 获取指定类型所有套打模板信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取指定类型所有套打模板信息")]
        public async Task<AjaxResult> GetAllByType(TemplateType type)
        {
            return await AjaxResult.Business(async result =>
            {
                result.Success(await _printContract.GetAllByTypeAsync(type));
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
    }
}
