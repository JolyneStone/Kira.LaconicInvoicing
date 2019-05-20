using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Kira.LaconicInvoicing.Common;
using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using OSharp.Entity;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public class StatisticsService: IStatisticsContract
    {
        private readonly IRepository<SaleOrder, Guid> _saleOrderRepo;
        private readonly IRepository<SaleOrderItem, Guid> _saleOrderItemRepo;
        private readonly ILogger<StatisticsService> _logger;

        public StatisticsService(IRepository<SaleOrder, Guid> saleOrderRepo,
            IRepository<SaleOrderItem, Guid> saleOrderItemRepo,
            ILogger<StatisticsService> logger)
        {
            _saleOrderRepo = saleOrderRepo;
            _saleOrderItemRepo = saleOrderItemRepo;
            _logger = logger;
        }

        public async Task<ValueTuple<ColumnChartDto, ColumnChartDto>> TrendStatisticsAnalysisAsync(DateTime startDate, DateTime endDate, StatisticsPeriod period)
        {
            var datas = await _saleOrderRepo.Query()
                        .Where(po => po.DateTime != null && po.DateTime >= startDate && po.DateTime <= endDate)
                        .Select(po => new
                        {
                            X = period == StatisticsPeriod.Month ? po.DateTime.Value.Month : po.DateTime.Value.Year,
                            Amount = po.TotalAmount,
                            Quantity = po.TotalQuantity
                        })
                        .GroupBy(o => o.X)
                        .OrderBy(o => o.Key)
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
            var customerDatas = await _saleOrderRepo.Query()
                              .Where(po => po.DateTime != null && po.DateTime >= startDate && po.DateTime <= endDate)
                              .Join(serviceProvider.GetService<IRepository<Customer, Guid>>().Query(),
                                    po => po.CustomerNumber,
                                    customer => customer.Number,
                                    (po, customer) => new
                                    {
                                        Type = customer.Type,
                                        TotalAmount = po.TotalAmount,
                                        TotalQuantity = po.TotalQuantity
                                    })
                              .GroupBy(o => o.Type)
                              .Select(o => new
                              {
                                  Type = o.Key,
                                  TotalAmount = o.Sum(t => t.TotalAmount),
                                  TotalQuantity = o.Sum(t => t.TotalQuantity)
                              })
                              .ToListAsync();


            var customerAmountPie = new PieChartDto
            {
                Data = customerDatas
                        .Select(d => new PieDto
                        {
                            Name = d.Type,
                            Ratio = d.TotalAmount
                        })
                        .ToList()
            };

            var customerQuantityPie = new PieChartDto
            {
                Data = customerDatas
                         .Select(d => new PieDto
                         {
                             Name = d.Type,
                             Ratio = d.TotalQuantity
                         })
                         .ToList()
            };

            var productDatas = await _saleOrderRepo.Query()
            .Where(po => po.DateTime != null && po.DateTime >= startDate && po.DateTime <= endDate)
            .Join(_saleOrderItemRepo.Query(),
                po => po.Id,
                poi => poi.SaleOrderId,
                (po, poi) => new
                {
                    ProductNumber = poi.Number,
                    ProductAmount = poi.Amount,
                    ProductPrice = poi.Price
                })
                .Join(serviceProvider.GetService<IRepository<Product, Guid>>().Query(),
                    p => p.ProductNumber,
                    product => product.Number,
                    (p, product) => new
                    {
                        Type = product.Type,
                        TotalAmount = p.ProductAmount * p.ProductPrice,
                        TotalQuantity = p.ProductAmount
                    })
                    .GroupBy(o => o.Type)
                    .Select(o => new
                    {
                        Type = o.Key,
                        TotalAmount = o.Sum(t => t.TotalAmount),
                        TotalQuantity = o.Sum(t => t.TotalQuantity)
                    })
                    .ToListAsync();

            var productAmountPie = new PieChartDto
            {
                Data = productDatas
                        .Select(d => new PieDto
                        {
                            Name = d.Type,
                            Ratio = d.TotalAmount
                        })
                        .ToList()
            };

            var productQuantityPie = new PieChartDto
            {
                Data = productDatas
                        .Select(d => new PieDto
                        {
                            Name = d.Type,
                            Ratio = d.TotalQuantity
                        })
                        .ToList()
            };

            return (customerAmountPie, customerQuantityPie, productAmountPie, productQuantityPie);
        }
    }
}
