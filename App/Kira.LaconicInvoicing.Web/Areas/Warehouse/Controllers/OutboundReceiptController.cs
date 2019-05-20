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
    [Description("管理-出库单信息")]
    public class OutboundReceiptController : WarehouseApiController
    {
        private readonly IOutboundReceiptContract _outboundReceiptContract;

        /// <summary>
        /// 初始化一个<see cref="OutboundReceiptController"/>类型的新实例
        /// </summary>
        public OutboundReceiptController(IOutboundReceiptContract outboundReceiptContract)
        {
            _outboundReceiptContract = outboundReceiptContract;
        }

        /// <summary>
        /// 获取新出库单编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新出库单编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _outboundReceiptContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询出库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询出库单信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _outboundReceiptContract.SearchAsync(query?.ToQueryCondition<OutboundReceipt>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加出库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加出库单信息")]
        public async Task<AjaxResult> Add([FromBody]OutboundReceiptInputDto dto)
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
                    result.Error("出库单至少需要一个采购项");
                    return;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                foreach (var item in dto.Items)
                {
                    item.Id = Guid.NewGuid();
                    item.OutboundReceiptId = dto.Id;
                }

                var isSuccess = await _outboundReceiptContract.AddOutboundReceiptAsync(dto, ServiceProvider);
                result.Type = isSuccess? AjaxResultType.Success:AjaxResultType.Error;
            });
        }

        /// <summary>
        /// 根据指定Id获取出库单信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取出库单信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _outboundReceiptContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的出库单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新出库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新出库单信息")]
        public async Task<AjaxResult> Update([FromBody]OutboundReceiptInputDto dto)
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
                    item.OutboundReceiptId = dto.Id;
                }
                await _outboundReceiptContract.UpdateOutboundReceiptAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的出库单信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id出库单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id出库单信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _outboundReceiptContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的出库单信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取出库单打印信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取出库单打印信息")]
        public async Task<AjaxResult> GetPrintData(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _outboundReceiptContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的出库单信息");
                }
                else
                {
                    result.Success(dto.MapTo<OutboundReceiptPrintDataDto>());
                }
            });
        }
    }
}
