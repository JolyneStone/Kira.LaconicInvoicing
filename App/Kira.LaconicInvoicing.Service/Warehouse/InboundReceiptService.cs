using Kira.LaconicInvoicing.Service.BaseData;
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

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public class InboundReceiptService : IInboundReceiptContract
    {
        private readonly IRepository<InboundReceipt, Guid> _inboundReceiptRepo;
        private readonly IRepository<InboundReceiptItem, Guid> _inboundReceiptItemRepo;
        private readonly ILogger<InboundReceiptService> _logger;

        public InboundReceiptService(IRepository<InboundReceipt, Guid> inboundReceiptRepo,
              IRepository<InboundReceiptItem, Guid> inboundReceiptItemRepo,
              ILogger<InboundReceiptService> logger)
        {
            _inboundReceiptRepo = inboundReceiptRepo;
            _inboundReceiptItemRepo = inboundReceiptItemRepo;
            _logger = logger;
        }

        public async Task<PageData<InboundReceiptOutputDto>> SearchAsync(QueryCondition<InboundReceipt> condition = null)
        {
            var query = _inboundReceiptRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<InboundReceiptOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("InboundReceipt", out prefix))
            {
                throw new BussinessException("无法找到入库单的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _inboundReceiptRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddInboundReceiptAsync(InboundReceiptInputDto dto, IServiceProvider serviceProvider)
        {
            if (dto.Items == null || dto.Items.Length <= 0)
                return false;

            var inboundReceipt = dto.MapTo<InboundReceipt>();
            var items = dto.Items.Select(d => d.MapTo<InboundReceiptItem>()).ToArray();

            var inventoryRepo = serviceProvider.GetService<IRepository<Inventory, Guid>>();
            var warehouseRepo = serviceProvider.GetService<IRepository<Kira.LaconicInvoicing.Warehouse.Entities.Warehouse, Guid>>();
            var warehouse = await warehouseRepo.GetFirstAsync(w => w.Number == dto.WarehouseNumber);

            var inventories = inventoryRepo.Query().Where(iv => iv.WarehouseId == warehouse.Id);
            var updateInventories = new List<Inventory>();
            var addInventories = new List<Inventory>();

            var dt = DateTime.Now;
            foreach(var item in items)
            {
                if (inventories.Any(iv => iv.Number == item.Number))
                {
                    var inventory = await inventories.FirstOrDefaultAsync(iv => iv.Number == item.Number);
                    inventory.Amount += item.Amount;
                    inventory.DateTime = dt;
                    updateInventories.Add(inventory);
                }
                else
                {
                    var inventory = new Inventory()
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = warehouse.Id,
                        GoodsCategory = item.GoodsCategory,
                        Name = item.Name,
                        Number = item.Number,
                        Amount = item.Amount,
                        DateTime = dt
                    };

                    addInventories.Add(inventory);
                }
            }

            await _inboundReceiptRepo.InsertAsync(inboundReceipt);
            await _inboundReceiptItemRepo.InsertAsync(items);
            if (updateInventories.Count > 0)
                await inventoryRepo.UpdateAsync(updateInventories.ToArray());
            if (addInventories.Count > 0)
                await inventoryRepo.InsertAsync(addInventories.ToArray());
            return true;
        }

        public async Task<InboundReceiptOutputDto> GetAsync(Guid id)
        {
            var inboundReceipt = (await _inboundReceiptRepo.GetAsync(id)).MapTo<InboundReceiptOutputDto>();
            inboundReceipt.Items = (await _inboundReceiptItemRepo.Query().Where(p => p.InboundReceiptId == id).ToListAsync())
             .Select(p => p.MapTo<InboundReceiptItemOutputDto>())
             .ToArray();
            return inboundReceipt;
        }

        public async Task<bool> UpdateInboundReceiptAsync(InboundReceiptInputDto dto)
        {
            var inboundReceipt = dto.MapTo<InboundReceipt>();
            await _inboundReceiptRepo.UpdateAsync(inboundReceipt);
            var items = dto.Items.Select(d => d.MapTo<InboundReceiptItem>()).ToArray();
            await _inboundReceiptItemRepo.UpdateAsync(items);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _inboundReceiptRepo.DeleteAsync(ids);
            await _inboundReceiptItemRepo.DeleteBatchAsync(p => ids.Contains(p.InboundReceiptId));
            return result.Successed;
        }
    }
}
