using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Service.Purcachase;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Purchase.Controllers
{
    [ModuleInfo(Order = 1, Position = "Purchase", PositionName = "采购模块")]
    [Description("管理-采购单信息")]
    public class PurchaseOrderController : PurchaseApiController
    {
        private readonly IPurchaseOrderContract _purchaseOrderContract;

        /// <summary>
        /// 初始化一个<see cref="PurchaseOrderController"/>类型的新实例
        /// </summary>
        public PurchaseOrderController(IPurchaseOrderContract purchaseOrderContract)
        {
            _purchaseOrderContract = purchaseOrderContract;
        }

        /// <summary>
        /// 获取新采购单编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新采购单编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _purchaseOrderContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询采购单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询采购单信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _purchaseOrderContract.SearchAsync(query?.ToQueryCondition<PurchaseOrder>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加采购单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加采购单信息")]
        public async Task<AjaxResult> Add([FromBody]PurchaseOrderInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                Check.NotNull(dto.Items, nameof(dto.Items));
                dto.Id = Guid.NewGuid();
                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                if (dto.Items == null || dto.Items.Length <= 0)
                {
                    result.Error("采购单至少需要一个采购项");
                    return;
                }

                foreach (var item in dto.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.PurchaseOrderId = dto.Id;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                await _purchaseOrderContract.AddPurchaseOrderAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取采购单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取采购单信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _purchaseOrderContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的采购单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新采购单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新采购单信息")]
        public async Task<AjaxResult> Update([FromBody]PurchaseOrderInputDto dto)
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
                foreach (var item in dto.Items)
                {
                    item.PurchaseOrderId = dto.Id;
                }
                await _purchaseOrderContract.UpdatePurchaseOrderAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的采购单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id采购单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id采购单信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _purchaseOrderContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的采购单信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取采购单打印信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取采购单打印信息")]
        public async Task<AjaxResult> GetPrintData(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _purchaseOrderContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的采购单信息");
                }
                else
                {
                    result.Success(dto.MapTo<PurchaseOrderPrintDataDto>());
                }
            });
        }
    }
}
