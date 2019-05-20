using Kira.LaconicInvoicing.Service.Warehouse;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Mapping;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Warehouse.Controllers
{
    [ModuleInfo(Order = 1, Position = "Warehouse", PositionName = "仓库模块")]
    [Description("管理-入库单信息")]
    public class InboundReceiptController : WarehouseApiController
    {
        private readonly IInboundReceiptContract _inboundReceiptContract;

        /// <summary>
        /// 初始化一个<see cref="InboundReceiptController"/>类型的新实例
        /// </summary>
        public InboundReceiptController(IInboundReceiptContract inboundReceiptContract)
        {
            _inboundReceiptContract = inboundReceiptContract;
        }

        /// <summary>
        /// 获取新入库单编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新入库单编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _inboundReceiptContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询入库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询入库单信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _inboundReceiptContract.SearchAsync(query?.ToQueryCondition<InboundReceipt>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加入库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加入库单信息")]
        public async Task<AjaxResult> Add([FromBody]InboundReceiptInputDto dto)
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
                if (dto.Items == null || dto.Items.Length <= 0)
                {
                    result.Error("入库单至少需要一个采购项");
                    return;
                }

                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                foreach (var item in dto.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.InboundReceiptId = dto.Id;
                }
                await _inboundReceiptContract.AddInboundReceiptAsync(dto, ServiceProvider);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取入库单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取入库单信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _inboundReceiptContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的入库单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新入库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新入库单信息")]
        public async Task<AjaxResult> Update([FromBody]InboundReceiptInputDto dto)
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
                    item.InboundReceiptId = dto.Id;
                }
                await _inboundReceiptContract.UpdateInboundReceiptAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的入库单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id入库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id入库单信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _inboundReceiptContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的入库单信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取入库单打印信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取入库单打印信息")]
        public async Task<AjaxResult> GetPrintData(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _inboundReceiptContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的入库单信息");
                }
                else
                {
                    result.Success(dto.MapTo<InboundReceiptPrintDataDto>());
                }
            });
        }
    }
}
