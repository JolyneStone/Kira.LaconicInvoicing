using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Service.Warehouse;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using WarehouseEntity = Kira.LaconicInvoicing.Warehouse.Entities.Warehouse;

namespace Kira.LaconicInvoicing.Web.Areas.Warehouse.Controllers
{
    [ModuleInfo(Order = 1, Position = "Warehouse", PositionName = "仓库模块")]
    [Description("管理-仓库信息")]
    public class WarehouseController: WarehouseApiController
    {
        private readonly IWarehouseContract _warehouseContract;

        /// <summary>
        /// 初始化一个<see cref="WarehouseController"/>类型的新实例
        /// </summary>
        public WarehouseController(IWarehouseContract warehouseContract)
        {
            _warehouseContract = warehouseContract;
        }

        /// <summary>
        /// 获取新仓库编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新仓库编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _warehouseContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询仓库信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询仓库信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _warehouseContract.SearchAsync(query?.ToQueryCondition<WarehouseEntity>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加仓库信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加仓库信息")]
        public async Task<AjaxResult> Add([FromBody]WarehouseInputDto dto)
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
                await _warehouseContract.AddWarehouseAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取仓库信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取仓库信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _warehouseContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的仓库信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新仓库信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新仓库信息")]
        public async Task<AjaxResult> Update([FromBody]WarehouseInputDto dto)
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
                await _warehouseContract.UpdateWarehouseAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的仓库信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id仓库信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id仓库信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _warehouseContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的仓库信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

    }
}
