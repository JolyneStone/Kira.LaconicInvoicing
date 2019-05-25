using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Service.BaseData;
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

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public class PurchaseOrderService : IPurchaseOrderContract
    {
        private readonly IRepository<PurchaseOrder, Guid> _purchaseOrderRepo;
        private readonly IRepository<PurchaseOrderItem, Guid> _purchaseOrderItemRepo;
        private readonly ILogger<PurchaseOrderService> _logger;

        public PurchaseOrderService(IRepository<PurchaseOrder, Guid> purchaseOrderRepo,
            IRepository<PurchaseOrderItem, Guid> purchaseOrderItemRepo,
            ILogger<PurchaseOrderService> logger)
        {
            _purchaseOrderRepo = purchaseOrderRepo;
            _purchaseOrderItemRepo = purchaseOrderItemRepo;
            _logger = logger;
        }

        public async Task<PageData<PurchaseOrderOutputDto>> SearchAsync(QueryCondition<PurchaseOrder> condition = null)
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

            return (await query.ToListAsync()).Select(v => v.MapTo<PurchaseOrderOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("PurchaseOrder", out prefix))
            {
                throw new BussinessException("无法找到采购单的编号前缀");
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

        public async Task<bool> AddPurchaseOrderAsync(PurchaseOrderInputDto dto)
        {
            if (dto.Items == null || dto.Items.Length <= 0)
                return false;

            var purchaseOrder = dto.MapTo<PurchaseOrder>();
            var items = dto.Items.Select(d => d.MapTo<PurchaseOrderItem>()).ToArray();
            //foreach(var item in items)
            //{
            //    item.PurchaseOrderId = purchaseOrder.Id;
            //}
            await _purchaseOrderRepo.InsertAsync(purchaseOrder);
            await _purchaseOrderItemRepo.InsertAsync(items);
            return true;
        }

        public async Task<PurchaseOrderOutputDto> GetAsync(Guid id)
        {
            var purchaseOrder = (await _purchaseOrderRepo.GetAsync(id)).MapTo<PurchaseOrderOutputDto>();
            purchaseOrder.Items = (await _purchaseOrderItemRepo.Query().Where(p => p.PurchaseOrderId == id).ToListAsync())
                .Select(p => p.MapTo<PurchaseOrderItemOutputDto>())
                .ToArray();

            return purchaseOrder;
        }

        public async Task<bool> UpdatePurchaseOrderAsync(PurchaseOrderInputDto dto)
        {
            var purchaseOrder = dto.MapTo<PurchaseOrder>();
            await _purchaseOrderRepo.UpdateAsync(purchaseOrder);
            var items = dto.Items.Select(d => d.MapTo<PurchaseOrderItem>()).ToArray();
            await _purchaseOrderItemRepo.UpdateAsync(items);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            await _purchaseOrderRepo.DeleteAsync(ids);
            await _purchaseOrderItemRepo.DeleteBatchAsync(p => ids.Contains(p.PurchaseOrderId));
            return true;
        }
    }
}
