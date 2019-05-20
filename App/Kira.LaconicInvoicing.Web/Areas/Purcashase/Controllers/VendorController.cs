using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Service.Purcachase;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Purchase.Controllers
{
    [ModuleInfo(Order = 1, Position = "Purchase", PositionName = "采购模块")]
    [Description("管理-供应商信息")]
    public class VendorController : PurchaseApiController
    {
        private readonly IVendorContract _vendorContract;

        /// <summary>
        /// 初始化一个<see cref="VendorController"/>类型的新实例
        /// </summary>
        public VendorController(IVendorContract vendorContract)
        {
            _vendorContract = vendorContract;
        }

        /// <summary>
        /// 获取新供应商编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新供应商编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _vendorContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询供应商信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询供应商信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
             {
                 var data = await _vendorContract.SearchAsync(query?.ToQueryCondition<Vendor>());
                 return AjaxResult.CreateSuccess(data);
             });
        }

        /// <summary>
        /// 添加供应商信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加供应商信息")]
        public async Task<AjaxResult> Add([FromBody]VendorInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                dto.Id = Guid.NewGuid();
                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                await _vendorContract.AddVendorAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取供应商信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取供应商信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _vendorContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的供应商信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新供应商信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新供应商信息")]
        public async Task<AjaxResult> Update([FromBody]VendorInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));

                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                await _vendorContract.UpdateVendorAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的供应商信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id供应商信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id供应商信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _vendorContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的供应商信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取指定供应商的原料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("获取指定供应商的原料")]
        public async Task<AjaxResult> GetMaterials([FromBody]QueryContainer query)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(query, nameof(query));
                var queryCondition = query.ToQueryCondition<VendorMaterial>();
                foreach (var f in queryCondition.Filters)
                {
                    if (!string.IsNullOrWhiteSpace(f.Field) && f.Field.EqualsIgnoreCase("VendorId"))
                    {
                        f.Value = Guid.Parse(f.Value.ToString());
                    }
                }
                var data = await _vendorContract.GetMaterials(queryCondition, ServiceProvider);
                result.Success(data);
            });
        }

        /// <summary>
        /// 更新指定供应商的原料信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新指定供应商的原料信息")]
        public async Task<AjaxResult> UpdateMaterials([FromBody]RelevantDto<Guid, Guid> dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                Check.NotNull(dto.Id, nameof(dto.Id));
                Check.NotNull(dto.Ids, nameof(dto.Ids));

                var r = await _vendorContract.UpdateMaterials(dto.Id, dto.Ids, ServiceProvider);
                if (r == false)
                {
                    result.Error("无法更新指定供应商的原料信息");
                }
                else
                {
                    result.Success();
                }
            });
        }
    }
}
