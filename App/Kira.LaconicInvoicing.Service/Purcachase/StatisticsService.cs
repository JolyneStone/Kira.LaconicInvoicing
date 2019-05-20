using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Kira.LaconicInvoicing.Common;

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public class StatisticsService : IStatisticsContract
    {
        private readonly IRepository<PurchaseOrder, Guid> _purchaseOrderRepo;
        private readonly IRepository<PurchaseOrderItem, Guid> _purchaseOrderItemRepo;
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(IRepository<PurchaseOrder, Guid> purchaseOrderRepo,
            IRepository<PurchaseOrderItem, Guid> purchaseOrderItemRepo,
            ILogger<StatisticsService> logger)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _purchaseOrderItemRepo = purchaseOrderItemRepo;
            _logger = logger;
        }

        public async Task<ValueTuple<ColumnChartDto, ColumnChartDto>> TrendStatisticsAnalysisAsync(DateTime startDate, DateTime endDate, StatisticsPeriod period)
        {
            var datas = await _purchaseOrderRepo.Query()
                        .Where(po => po.DateTime != null && po.DateTime >= startDate && po.DateTime <= endDate)
                        .Select(po => new
                        {
                            X = period == StatisticsPeriod.Month ? po.DateTime.Value.Month : po.DateTime.Value.Year,
                            Amount = po.TotalAmount,
                            Quantity = po.TotalQuantity
                        })
                        .GroupBy(o => o.X)
                        .OrderBy(o=>o.Key)
                        .ToListAsync();

            var amountColumn = new ColumnChartDto
            {
                Data = datas.Select(d => new ColumnDto
                {
                    Xpos = d.Key.ToString(),
                    Ypos = d.Sum(t => t.Amount)
                }).ToList()
            };

            var quantityColumn = new ColumnChartDto
            {
                Data = datas.Select(d => new ColumnDto
                {
                    Xpos = d.Key.ToString(),
                    Ypos = d.Sum(t => t.Quantity)
                })
                .ToList()
            };

            return (amountColumn, quantityColumn);
        }

        public async Task<ValueTuple<PieChartDto, PieChartDto, PieChartDto, PieChartDto>> GeneralStatisticsAnalysisAsync(
            IServiceProvider serviceProvider,
            DateTime startDate,
            DateTime endDate)
        {
            var vendorDatas = await _purchaseOrderRepo.Query()
                              .Where(po => po.DateTime != null && po.DateTime >= startDate && po.DateTime <= endDate)
                              .Join(serviceProvider.GetService<IRepository<Vendor, Guid>>().Query(),
                                    po => po.VendorNumber,
                                    vendor => vendor.Number,
                                    (po, vendor) => new
                                    {
                                        Type = vendor.Type,
                                        TotalAmount = po.TotalAmount,
                                        TotalQuantity = po.TotalQuantity
                                    })
                              .GroupBy(o=>o.Type)
                              .Select(o => new
                              {
                                  Type = o.Key,
                                  TotalAmount = o.Sum(t=>t.TotalAmount),
                                  TotalQuantity =o.Sum(t=>t.TotalQuantity)
                              })
                              .ToListAsync();
                              

            var vendorAmountPie = new PieChartDto
            {
                Data = vendorDatas
                        .Select(d => new PieDto
                        {
                            Name = d.Type,
                            Ratio = d.TotalAmount
                        })
                        .ToList()
            };

            var vendorQuantityPie = new PieChartDto
            {
                Data = vendorDatas
                         .Select(d => new PieDto
                         {
                             Name = d.Type,
                             Ratio = d.TotalQuantity
                         })
                         .ToList()
            };

            var materialDatas = await _purchaseOrderRepo.Query()
            .Where(po => po.DateTime != null && po.DateTime >= startDate && po.DateTime <= endDate)
            .Join(_purchaseOrderItemRepo.Query(),
                po => po.Id,
                poi => poi.PurchaseOrderId,
                (po, poi) => new
                {
                    MaterialNumber = poi.Number,
                    MaterialAmount = poi.Amount,
                    MaterialPrice = poi.Price
                })
                .Join(serviceProvider.GetService<IRepository<Material, Guid>>().Query(),
                    p => p.MaterialNumber,
                    material => material.Number,
                    (p, material) => new
                    {
                        Type = material.Type,
                        TotalAmount = p.MaterialAmount * p.MaterialPrice,
                        TotalQuantity = p.MaterialAmount
                    })
                    .GroupBy(o=>o.Type)
                    .Select(o=>new
                    {
                        Type = o.Key,
                        TotalAmount = o.Sum(t => t.TotalAmount),
                        TotalQuantity = o.Sum(t => t.TotalQuantity)
                    })
                    .ToListAsync();

            var materialAmountPie = new PieChartDto
            {
                Data = materialDatas
                        .Select(d => new PieDto
                        {
                            Name = d.Type,
                            Ratio = d.TotalAmount
                        })
                        .ToList()
            };

            var materialQuantityPie = new PieChartDto
            {
                Data = materialDatas
                        .Select(d => new PieDto
                        {
                            Name = d.Type,
                            Ratio = d.TotalQuantity
                        })
                        .ToList()
            };

            return (vendorAmountPie, vendorQuantityPie, materialAmountPie, materialQuantityPie);
        }
    }
}
