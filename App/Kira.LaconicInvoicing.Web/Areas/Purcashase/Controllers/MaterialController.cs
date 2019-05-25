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
    [Description("管理-原料信息")]
    public class MaterialController : PurchaseApiController
    {
        private readonly IMaterialContract _materialContract;

        /// <summary>
        /// 初始化一个<see cref="MaterialController"/>类型的新实例
        /// </summary>
        public MaterialController(IMaterialContract materialContract)
        {
            _materialContract = materialContract;
        }

        /// <summary>
        /// 获取新原料编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新原料编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _materialContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询原料信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询原料信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _materialContract.SearchAsync(query?.ToQueryCondition<Material>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加原料信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加原料信息")]
        public async Task<AjaxResult> Add([FromBody]MaterialInputDto dto)
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
                await _materialContract.AddMaterialAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取原料信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取原料信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _materialContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的原料信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新原料信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新原料信息")]
        public async Task<AjaxResult> Update([FromBody]MaterialInputDto dto)
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
                await _materialContract.UpdateMaterialAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的原料信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id原料信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id原料信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _materialContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的原料信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取指定原料的原料
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("获取指定原料的供应商")]
        public async Task<AjaxResult> GetVendors([FromBody]QueryContainer query)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(query, nameof(query));
                var queryCondition = query.ToQueryCondition<VendorMaterial>();
                foreach (var f in queryCondition.Filters)
                {
                    if (!string.IsNullOrWhiteSpace(f.Field) && f.Field.EqualsIgnoreCase("MaterialId"))
                    {
                        f.Value = Guid.Parse(f.Value.ToString());
                    }
                }
                var data = await _materialContract.GetVendors(queryCondition, ServiceProvider);
                result.Success(data);
            });
        }

        /// <summary>
        /// 更新指定原料的供应商信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新指定原料的供应商信息")]
        public async Task<AjaxResult> UpdateVendors([FromBody]RelevantDto<Guid, Guid> dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                Check.NotNull(dto.Id, nameof(dto.Id));
                Check.NotNull(dto.Ids, nameof(dto.Ids));
                dto.Ids = dto.Ids.Distinct().ToArray();
                var r = await _materialContract.UpdateVendors(dto.Id, dto.Ids, ServiceProvider);
                if (r == false)
                {
                    result.Error("无法更新指定原料的供应商信息");
                }
                else
                {
                    result.Success();
                }
            });
        }
    }
}
