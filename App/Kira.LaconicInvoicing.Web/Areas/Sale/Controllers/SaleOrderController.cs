using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using Kira.LaconicInvoicing.Service.Sale;
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

namespace Kira.LaconicInvoicing.Web.Areas.Sale.Controllers
{
    [ModuleInfo(Order = 1, Position = "Sale", PositionName = "销售模块")]
    [Description("管理-销售单信息")]
    public class SaleOrderController : SaleApiController
    {
        private readonly ISaleOrderContract _saleOrderContract;

        /// <summary>
        /// 初始化一个<see cref="SaleOrderController"/>类型的新实例
        /// </summary>
        public SaleOrderController(ISaleOrderContract saleOrderContract)
        {
            _saleOrderContract = saleOrderContract;
        }

        /// <summary>
        /// 获取新销售单编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新销售单编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _saleOrderContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询销售单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询销售单信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _saleOrderContract.SearchAsync(query?.ToQueryCondition<SaleOrder>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加销售单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加销售单信息")]
        public async Task<AjaxResult> Add([FromBody]SaleOrderInputDto dto)
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
                    result.Error("销售单至少需要一个销售项");
                    return;
                }
                foreach (var item in dto.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.SaleOrderId = dto.Id;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                await _saleOrderContract.AddSaleOrderAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取销售单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取销售单信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _saleOrderContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的销售单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新销售单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新销售单信息")]
        public async Task<AjaxResult> Update([FromBody]SaleOrderInputDto dto)
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
                    item.SaleOrderId = dto.Id;
                }
                await _saleOrderContract.UpdateSaleOrderAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的销售单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id销售单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id销售单信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _saleOrderContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的销售单信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取销售单打印信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取销售单打印信息")]
        public async Task<AjaxResult> GetPrintData(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _saleOrderContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的销售单信息");
                }
                else
                {
                    result.Success(dto.MapTo<SaleOrderPrintDataDto>());
                }
            });
        }
    }
}
