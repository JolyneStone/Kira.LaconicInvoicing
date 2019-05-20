using Kira.LaconicInvoicing.Service.Warehouse;
using Kira.LaconicInvoicing.Warehouse;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
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

namespace Kira.LaconicInvoicing.Web.Areas.Warehouse.Controllers
{
    [ModuleInfo(Order = 2, Position = "Warehouse", PositionName = "仓库模块")]
    [Description("管理-库存信息")]
    public class InventoryController : WarehouseApiController
    {
        private readonly IInventoryContract _inventoryContract;

        /// <summary>
        /// 初始化一个<see cref="InventoryController"/>类型的新实例
        /// </summary>
        public InventoryController(IInventoryContract inventoryContract)
        {
            _inventoryContract = inventoryContract;
        }

        /// <summary>
        /// 查询库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询库存信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
            {
                if (query != null)
                {
                    foreach (var f in query.Filters)
                    {
                        if (!string.IsNullOrWhiteSpace(f.Field) && f.Field.EqualsIgnoreCase("WarehouseId"))
                        {
                            f.Value = Guid.Parse(f.Value.ToString());
                        }
                    }
                }
                var data = await _inventoryContract.SearchAsync(query?.ToQueryCondition<Inventory>());
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询原料库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询原料库存信息")]
        public async Task<AjaxResult> SearchMaterial([FromBody]QueryContainer query)
        {
            return await AjaxResult.Business(async () =>
            {
                foreach (var f in query.Filters)
                {
                    if (!string.IsNullOrWhiteSpace(f.Field) && f.Field.EqualsIgnoreCase("WarehouseId"))
                    {
                        f.Value = Guid.Parse(f.Value.ToString());
                    }
                }

                if(query.Filters != null)
                {
                    if(query.Filters.All(f=> !string.IsNullOrWhiteSpace(f.Field) && !f.Field.EqualsIgnoreCase("GoodsCategory")))
                    {
                        query.Filters.Add(new QueryConditionDescription()
                        {
                            Field = "GoodsCategory",
                            ConditionOp = "Equal",
                            LogicOp = "AndAlso",
                            Value = GoodsCategory.Material
                        });
                    }
                }
                else
                {
                    query.Filters = new List<QueryConditionDescription>()
                    {
                        new QueryConditionDescription()
                        {
                            Field = "GoodsCategory",
                            ConditionOp = "Equal",
                            LogicOp = "AndAslo",
                            Value = GoodsCategory.Material
                        }
                    };
                }

                var data = await _inventoryContract.SearchMaterialAsync(query?.ToQueryCondition<Inventory>(), ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询产品库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询产品库存信息")]
        public async Task<AjaxResult> SearchProduct([FromBody]QueryContainer query)
        {
            return await AjaxResult.Business(async () =>
            {
                foreach (var f in query.Filters)
                {
                    if (!string.IsNullOrWhiteSpace(f.Field) && f.Field.EqualsIgnoreCase("WarehouseId"))
                    {
                        f.Value = Guid.Parse(f.Value.ToString());
                    }
                }

                if (query.Filters != null)
                {
                    if (query.Filters.All(f => !string.IsNullOrWhiteSpace(f.Field) && !f.Field.EqualsIgnoreCase("GoodsCategory")))
                    {
                        query.Filters.Add(new QueryConditionDescription()
                        {
                            Field = "GoodsCategory",
                            ConditionOp = "Equal",
                            LogicOp = "AndAslo",
                            Value = GoodsCategory.Product
                        });
                    }
                }
                else
                {
                    query.Filters = new List<QueryConditionDescription>()
                    {
                        new QueryConditionDescription()
                        {
                            Field = "GoodsCategory",
                            ConditionOp = "Equal",
                            LogicOp = "AndAslo",
                            Value = GoodsCategory.Product
                        }
                    };
                }

                var data = await _inventoryContract.SearchProductAsync(query?.ToQueryCondition<Inventory>(), ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 添加库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加库存信息")]
        public async Task<AjaxResult> Add([FromBody]InventoryInputDto dto)
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

                dto.DateTime = DateTime.Now;
                await _inventoryContract.AddInventoryAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取库存信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取库存信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _inventoryContract.GetAsync(id, ServiceProvider);
                if (dto == null)
                {
                    result.Error("找不到指定的库存信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新库存信息")]
        public async Task<AjaxResult> Update([FromBody]InventoryInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));

                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }

                dto.DateTime = DateTime.Now;
                await _inventoryContract.UpdateInventoryAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的库存信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id库存信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id库存信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _inventoryContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的库存信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

    }
}
