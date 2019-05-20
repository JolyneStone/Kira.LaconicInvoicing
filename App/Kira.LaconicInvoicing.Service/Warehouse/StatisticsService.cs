using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Sale.Entities;
using Kira.LaconicInvoicing.Warehouse;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WarehouseEntity = Kira.LaconicInvoicing.Warehouse.Entities.Warehouse;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public class StatisticsService : IStatisticsContract
    {
        private readonly IRepository<WarehouseEntity, Guid> _warehouseRepo;
        private readonly IRepository<Inventory, Guid> _inventoryRepo;
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(IRepository<WarehouseEntity, Guid> warehouseRepo,
            IRepository<Inventory, Guid> inventoryRepo,
            ILogger<StatisticsService> logger)
        {
            _warehouseRepo = warehouseRepo;
            _inventoryRepo = inventoryRepo;
            _logger = logger;
        }

        public async Task<ValueTuple<PieChartDto, PieChartDto, PieChartDto>> StatisticsInventoryAnalysisAsync(
            IServiceProvider serviceProvider,
            Guid? id = null)
        {
            var materialRepo = serviceProvider.GetService<IRepository<Material, Guid>>();
            var productRepo = serviceProvider.GetService<IRepository<Product, Guid>>();

            if (id == null || id == Guid.Empty)
            {
                var query = await _warehouseRepo.Query()
                    .Join(_inventoryRepo.Query(),
                        warehouse => warehouse.Id,
                        inventory => inventory.WarehouseId,
                        (warehouse, inventory) => new
                        {
                            WarehouseName = warehouse.Name,
                            InventoryName = inventory.Name,
                            InventoryAmount = inventory.Amount,
                            InventoryNumber = inventory.Number,
                            Category = inventory.GoodsCategory,
                        }).ToListAsync();

                var warehousePie = new PieChartDto
                {
                    Data = query
                    .GroupBy(q => q.WarehouseName)
                    .Select(q => new PieDto
                    {
                        Name = q.Key,
                        Ratio = (double)q.Sum(t => t.InventoryAmount)
                    })
                    .ToList()
                };

                var materialPie = new PieChartDto
                {
                    Data = query.Where(q => q.Category == GoodsCategory.Material)
                        .Join(await materialRepo.Query().Select(material => new { Number = material.Number, Type = material.Type }).ToListAsync(),
                            q => q.InventoryNumber,
                            material => material.Number,
                            (q, material) => new PieDto
                            {
                                Name = material.Type,
                                Ratio = q.InventoryAmount
                            })
                            .GroupBy(p => p.Name)
                            .Select(p => new PieDto
                            {
                                Name = p.Key,
                                Ratio = p.Sum(t => t.Ratio)
                            }).ToList()
                };

                var productPie = new PieChartDto
                {
                    Data = query.Where(q => q.Category == GoodsCategory.Product)
                            .Join(await productRepo.Query().Select(product => new { Number = product.Number, Type = product.Type }).ToListAsync(),
                                q => q.InventoryNumber,
                                product => product.Number,
                                (q, product) => new PieDto
                                {
                                    Name = product.Type,
                                    Ratio = q.InventoryAmount
                                })
                                .GroupBy(q => q.Name)
                                .Select(p => new PieDto
                                {
                                    Name = p.Key,
                                    Ratio = p.Sum(t => t.Ratio)
                                }).ToList()
                };

                return (warehousePie, materialPie, productPie);
            }
            else
            {
                var query = await _inventoryRepo.Query().Where(inventory => inventory.WarehouseId == id).ToListAsync();

                var materialPie = new PieChartDto
                {
                    Data = query.Where(q => q.GoodsCategory == GoodsCategory.Material)
                     .Join(await materialRepo.Query().Select(material => new { Number = material.Number, Type = material.Type }).ToListAsync(),
                            q => q.Number,
                            material => material.Number,
                            (q, material) => new PieDto
                            {
                                Name = material.Type,
                                Ratio = q.Amount
                            })
                            .GroupBy(q => q.Name)
                            .Select(p => new PieDto
                            {
                                Name = p.Key,
                                Ratio = p.Sum(t => t.Ratio)
                            }).ToList()
                };

                var productPie = new PieChartDto
                {
                    Data = query.Where(q => q.GoodsCategory == GoodsCategory.Product)
                    .Join(await productRepo.Query().Select(product => new { Number = product.Number, Type = product.Type }).ToListAsync(),
                        q => q.Number,
                        product => product.Number,
                        (q, product) => new PieDto
                        {
                            Name = product.Type,
                            Ratio = q.Amount
                        })
                        .GroupBy(q => q.Name)
                        .Select(p => new PieDto
                        {
                            Name = p.Key,
                            Ratio = p.Sum(t => t.Ratio)
                        }).ToList()
                };

                return (null, materialPie, productPie);
            }
        }

        public async Task<IList<SimpleWarehouseDto>> GetWarehousesAsync()
        {
            return await _warehouseRepo.Query().Select(w => new SimpleWarehouseDto { Id = w.Id, Name = w.Name }).ToListAsync();
        }
    }
}
