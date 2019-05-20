using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using OSharp.Mapping;
using Kira.LaconicInvoicing.Service.BaseData;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public class SaleOrderService: ISaleOrderContract
    {
        private readonly IRepository<SaleOrder, Guid> _purchaseOrderRepo;
        private readonly IRepository<SaleOrderItem, Guid> _purchaseOrderItemRepo;
        private readonly ILogger<SaleOrderService> _logger;

        public SaleOrderService(IRepository<SaleOrder, Guid> purchaseOrderRepo,
            IRepository<SaleOrderItem, Guid> purchaseOrderItemRepo,
            ILogger<SaleOrderService> logger)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _purchaseOrderItemRepo = purchaseOrderItemRepo;
            _logger = logger;
        }

        public async Task<PageData<SaleOrderOutputDto>> SearchAsync(QueryCondition<SaleOrder> condition = null)
        {
            var query = _purchaseOrderRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<SaleOrderOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("SaleOrder", out prefix))
            {
                throw new BussinessException("无法找到销售单的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _purchaseOrderRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
            int number;
            if (string.IsNullOrWhiteSpace(maxNumber))
            {
                number = 0;
            }
            else
            {
                number = int.Parse(maxNumber.Substring(prefix.Length)) + 1;
            }

            return prefix + number.ToString().PadLeft(18 - prefix.Length, '0');
        }

        public async Task<bool> AddSaleOrderAsync(SaleOrderInputDto dto)
        {
            if (dto.Items == null || dto.Items.Length <= 0)
                return false;

            var purchaseOrder = dto.MapTo<SaleOrder>();
            var items = dto.Items.Select(d => d.MapTo<SaleOrderItem>()).ToArray();

            await _purchaseOrderRepo.InsertAsync(purchaseOrder);
            await _purchaseOrderItemRepo.InsertAsync(items);
            return true;
        }

        public async Task<SaleOrderOutputDto> GetAsync(Guid id)
        {
            var purchaseOrder = (await _purchaseOrderRepo.GetAsync(id)).MapTo<SaleOrderOutputDto>();
            purchaseOrder.Items = (await _purchaseOrderItemRepo.Query().Where(p => p.SaleOrderId == id).ToListAsync())
                .Select(p => p.MapTo<SaleOrderItemOutputDto>())
                .ToArray();

            return purchaseOrder;
        }

        public async Task<bool> UpdateSaleOrderAsync(SaleOrderInputDto dto)
        {
            var purchaseOrder = dto.MapTo<SaleOrder>();
            await _purchaseOrderRepo.UpdateAsync(purchaseOrder);
            var items = dto.Items.Select(d => d.MapTo<SaleOrderItem>()).ToArray();
            await _purchaseOrderItemRepo.UpdateAsync(items);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            await _purchaseOrderRepo.DeleteAsync(ids);
            await _purchaseOrderItemRepo.DeleteBatchAsync(p => ids.Contains(p.SaleOrderId));
            return true;
        }
    }
}
