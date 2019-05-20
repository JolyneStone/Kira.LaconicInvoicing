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
    public class OutboundReceiptService : IOutboundReceiptContract
    {
        private readonly IRepository<OutboundReceipt, Guid> _outboundReceiptRepo;
        private readonly IRepository<OutboundReceiptItem, Guid> _outboundReceiptItemRepo;
        private readonly ILogger<OutboundReceiptService> _logger;

        public OutboundReceiptService(IRepository<OutboundReceipt, Guid> outboundReceiptRepo,
              IRepository<OutboundReceiptItem, Guid> outboundReceiptItemRepo,
              ILogger<OutboundReceiptService> logger)
        {
            _outboundReceiptRepo = outboundReceiptRepo;
            _outboundReceiptItemRepo = outboundReceiptItemRepo;
            _logger = logger;
        }

        public async Task<PageData<OutboundReceiptOutputDto>> SearchAsync(QueryCondition<OutboundReceipt> condition = null)
        {
            var query = _outboundReceiptRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<OutboundReceiptOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("OutboundReceipt", out prefix))
            {
                throw new BussinessException("无法找到出库单的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _outboundReceiptRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddOutboundReceiptAsync(OutboundReceiptInputDto dto, IServiceProvider serviceProvider)
        {
            if (dto.Items == null || dto.Items.Length <= 0)
                return false;

            var outboundReceipt = dto.MapTo<OutboundReceipt>();
            var items = dto.Items.Select(d => d.MapTo<OutboundReceiptItem>()).ToArray();

            var inventoryRepo = serviceProvider.GetService<IRepository<Inventory, Guid>>();
            var warehouseRepo = serviceProvider.GetService<IRepository<Kira.LaconicInvoicing.Warehouse.Entities.Warehouse, Guid>>();
            var warehouse = await warehouseRepo.GetFirstAsync(w => w.Number == dto.WarehouseNumber);

            var inventories = inventoryRepo.Query().Where(iv => iv.WarehouseId == warehouse.Id);
            var updateInventories = new List<Inventory>();
            var addInventories = new List<Inventory>();

            var dt = DateTime.Now;
            foreach (var item in items)
            {
                if (inventories.Any(iv => iv.Number == item.Number))
                {
                    var inventory = await inventories.FirstOrDefaultAsync(iv => iv.Number == item.Number);
                    inventory.Amount -= item.Amount;
                    inventory.DateTime = dt;
                    if (inventory.Amount < 0)
                    {
                        throw new BussinessException("库存数不能少于0");
                    }
                    updateInventories.Add(inventory);
                }
                else
                {
                    var inventory = new Inventory()
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = warehouse.Id,
                        GoodsCategory = item.GoodsCategory,
                        Number = item.Number,
                        Amount = item.Amount,
                        DateTime = dt
                    };

                    addInventories.Add(inventory);
                }
            }

            await _outboundReceiptRepo.InsertAsync(outboundReceipt);
            await _outboundReceiptItemRepo.InsertAsync(items);

            if (updateInventories.Count > 0)
                await inventoryRepo.UpdateAsync(updateInventories.ToArray());
            if (addInventories.Count > 0)
                await inventoryRepo.InsertAsync(addInventories.ToArray());
            return true;
        }

        public async Task<OutboundReceiptOutputDto> GetAsync(Guid id)
        {
            var outboundReceipt = (await _outboundReceiptRepo.GetAsync(id)).MapTo<OutboundReceiptOutputDto>();
            outboundReceipt.Items = (await _outboundReceiptItemRepo.Query().Where(p => p.OutboundReceiptId == id).ToListAsync())
             .Select(p => p.MapTo<OutboundReceiptItemOutputDto>())
             .ToArray();
            return outboundReceipt;
        }

        public async Task<bool> UpdateOutboundReceiptAsync(OutboundReceiptInputDto dto)
        {
            var outboundReceipt = dto.MapTo<OutboundReceipt>();
            await _outboundReceiptRepo.UpdateAsync(outboundReceipt);
            var items = dto.Items.Select(d => d.MapTo<OutboundReceiptItem>()).ToArray();
            await _outboundReceiptItemRepo.UpdateAsync(items);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _outboundReceiptRepo.DeleteAsync(ids);
            await _outboundReceiptItemRepo.DeleteBatchAsync(p => ids.Contains(p.OutboundReceiptId));
            return result.Successed;
        }

    }
}
