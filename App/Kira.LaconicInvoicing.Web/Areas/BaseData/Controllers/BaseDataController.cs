using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Service.BaseData;
using Kira.LaconicInvoicing.Web.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.BaseData.Controllers
{
    [ModuleInfo(Order = 1, Position = "BaseData", PositionName = "基础数据模块")]
    [Description("管理-基础数据")]
    public class BaseDataController : BaseDataApiController
    {
        private readonly IBaseDataContract _baseDataContract;
        public BaseDataController(IBaseDataContract baseDataContract)
        {
            _baseDataContract = baseDataContract;
        }

        /// <summary>
        /// 获取基础数据列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取基础数据列表")]
        public async Task<List<BaseDataListDto>> GetValues(string type)
        {
            Check.NotNullOrEmpty(type, nameof(type));

            return (await _baseDataContract.GetListAsync(type))
                .Select(p => new BaseDataListDto { Name = p.Key, Code = p.Value })
                .ToList(); ;
        }

        /// <summary>
        /// 更新基础类型列表
        /// </summary>`
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("更新基础数据列表")]
        public async Task<AjaxResult> UpdateListValue(BaseDataListInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }

            await _baseDataContract.UpdateListAsync(dto.Type, dto.BaseData);
            return new AjaxResult(content: "更新基础数据成功", type: AjaxResultType.Success);
        }

        /// <summary>
        /// 添加基础数据列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加编号类型")]
        public async Task<AjaxResult> AddListValue(BaseDataListInputDto dto)
        {
            Check.NotNull(dto, nameof(dto));

            if (!ModelState.IsValid)
            {
                return new AjaxResult("提交信息验证失败", AjaxResultType.Error);
            }

            await _baseDataContract.AddListAsync(dto.Type, dto.BaseData);
            return new AjaxResult(content: "添加基础数据成功", type: AjaxResultType.Success);
        }

        /// <summary>
        /// 删除基础数据列表
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("删除基础数据列表")]
        public async Task<AjaxResult> DeleteListValue(string type, string name)
        {
            Check.NotNullOrEmpty(type, nameof(type));
            Check.NotNullOrEmpty(name, nameof(name));

            await _baseDataContract.DeleteListAsync(type, name);
            return new AjaxResult(content: "删除基础数据成功", type: AjaxResultType.Success);
        }

        /// <summary>
        /// 获取基础数据值
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取基础数据值")]
        public async Task<string> GetValue(string type)
        {
            Check.NotNullOrEmpty(type, nameof(type));

            return await _baseDataContract.GetValueAsync(type);
        }

        /// <summary>
        /// 更新基础数据值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("更新基础数据值")]
        public async Task<AjaxResult> UpdateValue(string type, string value)
        {
            Check.NotNullOrEmpty(type, nameof(type));
            Check.NotNullOrEmpty(value, nameof(value));

            await _baseDataContract.UpdateValueAsync(type, value);
            return new AjaxResult(content: "更新基础数据成功", type: AjaxResultType.Success);
        }
    }
}
