using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Service.BaseData;
using Kira.LaconicInvoicing.Warehouse;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Mapping;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kira.LaconicInvoicing.Sale.Entities;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public class InventoryService : IInventoryContract
    {
        private readonly IRepository<Inventory, Guid> _inventoryRepo;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(IRepository<Inventory, Guid> inventoryRepo,
              ILogger<InventoryService> logger)
        {
            _inventoryRepo = inventoryRepo;
            _logger = logger;
        }

        public async Task<PageData<InventoryOutputDto>> SearchAsync(QueryCondition<Inventory> condition = null)
        {
            var query = _inventoryRepo.Query();
            var total = 0;
            if (condition != null)
            {
                if (condition.Filters != null && condition.Filters.Count > 0)
                {
                    query = query.FilterDateTime(ref condition).Where(condition.GetFilter(), condition.GetValues());
                }

                if (condition.Sorts != null && condition.Sorts.Count > 0)
                {
                    query = query.OrderBy(condition.Sorts.ConvertToSortString());
                }

                total = await query.CountAsync();
                query = query.Skip((condition.PageIndex - 1) * condition.PageSize).Take(condition.PageSize);
            }

            return (await query.ToListAsync()).Select(v => v.MapTo<InventoryOutputDto>()).ToPageData(total);
        }

        public async Task<PageData<InventoryOutputDto>> SearchMaterialAsync(QueryCondition<Inventory> condition, IServiceProvider serviceProvider)
        {
            var query = _inventoryRepo.Query();
            var total = 0;
            if (condition.Filters != null && condition.Filters.Count > 0)
            {
                query = query.Where(condition.GetFilter(), condition.GetValues());
            }

            if (condition.Sorts != null && condition.Sorts.Count > 0)
            {
                query = query.OrderBy(condition.Sorts.ConvertToSortString());
            }

            total = await query.CountAsync();
            query = query.Skip((condition.PageIndex - 1) * condition.PageSize).Take(condition.PageSize);

            var materialRepo = serviceProvider.GetService<IRepository<Material, Guid>>();
            return (await (from inventory in query
                           join material in materialRepo.Query() on inventory.Number equals material.Number
                           select new InventoryOutputDto
                           {
                               Id = inventory.Id,
                               Number = inventory.Number,
                               GoodsCategory = GoodsCategory.Material,
                               Amount = inventory.Amount,
                               WarehouseId = inventory.WarehouseId,
                               DateTime = inventory.DateTime,
                               Name = inventory.Name,
                               Spec = material.Spec,
                               Price = material.Price,
                               Brand = material.Brand,
                               Unit = material.Unit,
                               Type = material.Type
                           }).ToListAsync()).ToPageData(total);
        }

        public async Task<PageData<InventoryOutputDto>> SearchProductAsync(QueryCondition<Inventory> condition, IServiceProvider serviceProvider)
        {
            var query = _inventoryRepo.Query();
            var total = 0;
            if (condition.Filters != null && condition.Filters.Count > 0)
            {
                query = query.Where(condition.GetFilter(), condition.GetValues());
            }

            if (condition.Sorts != null && condition.Sorts.Count > 0)
            {
                query = query.OrderBy(condition.Sorts.ConvertToSortString());
            }

            total = await query.CountAsync();
            query = query.Skip((condition.PageIndex - 1) * condition.PageSize).Take(condition.PageSize);

            var productRepo = serviceProvider.GetService<IRepository<Product, Guid>>();
            return (await (from inventory in query
                           join product in productRepo.Query() on inventory.Number equals product.Number
                           select new InventoryOutputDto
                           {
                               Id = inventory.Id,
                               Number = inventory.Number,
                               GoodsCategory = GoodsCategory.Material,
                               Amount = inventory.Amount,
                               WarehouseId = inventory.WarehouseId,
                               DateTime = inventory.DateTime,
                               Name = inventory.Name,
                               Spec = product.Spec,
                               Price = product.CostPrice,
                               Brand = product.Brand,
                               Unit = product.Unit,
                               Type = product.Type
                           }).ToListAsync()).ToPageData(total);
        }

        public async Task<bool> AddInventoryAsync(InventoryInputDto dto)
        {
            var inventory = dto.MapTo<Inventory>();
            await _inventoryRepo.InsertAsync(inventory);
            return true;
        }

        public async Task<InventoryOutputDto> GetAsync(Guid id, IServiceProvider serviceProvider)
        {
            var inventory = (await _inventoryRepo.GetAsync(id)).MapTo<InventoryOutputDto>();
            if (inventory == null)
                throw new BussinessException("无法找到指定库存信息");
            if (inventory.GoodsCategory == GoodsCategory.Material)
            {
                inventory = inventory.Map((await serviceProvider.GetService<IRepository<Material, Guid>>().GetFirstAsync(m => m.Number == inventory.Number)));
            }
            else
            {
                inventory = inventory.Map((await serviceProvider.GetService<IRepository<Product, Guid>>().GetFirstAsync(m => m.Number == inventory.Number)));
            }

            return inventory;
        }

        public async Task<bool> UpdateInventoryAsync(InventoryInputDto dto)
        {
            var inventory = dto.MapTo<Inventory>();
            await _inventoryRepo.UpdateAsync(inventory);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _inventoryRepo.DeleteAsync(ids);
            return result.Successed;
        }
    }
}
