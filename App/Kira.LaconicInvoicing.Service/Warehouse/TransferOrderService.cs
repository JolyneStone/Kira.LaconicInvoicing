using Kira.LaconicInvoicing.Service.BaseData;
using Kira.LaconicInvoicing.Warehouse.Dtos;
using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
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
    public class TransferOrderService: ITransferOrderContract
    {
        private readonly IRepository<TransferOrder, Guid> _transferOrderRepo;
        private readonly IRepository<TransferOrderItem, Guid> _transferOrderItemRepo;
        private readonly ILogger<TransferOrderService> _logger;

        public TransferOrderService(IRepository<TransferOrder, Guid> transferOrderRepo,
              IRepository<TransferOrderItem, Guid> transferOrderItemRepo,
              ILogger<TransferOrderService> logger)
        {
            _transferOrderRepo = transferOrderRepo;
            _transferOrderItemRepo = transferOrderItemRepo;
            _logger = logger;
        }

        public async Task<PageData<TransferOrderOutputDto>> SearchAsync(QueryCondition<TransferOrder> condition = null)
        {
            var query = _transferOrderRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<TransferOrderOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("TransferOrder", out prefix))
            {
                throw new BussinessException("无法找到调拨单的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _transferOrderRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddTransferOrderAsync(TransferOrderInputDto dto, IServiceProvider serviceProvider)
        {
            if (dto.Items == null || dto.Items.Length <= 0)
                return false;

            var transferOrder = dto.MapTo<TransferOrder>();
            var items = dto.Items.Select(d => d.MapTo<TransferOrderItem>()).ToArray();

            var inventoryRepo = serviceProvider.GetService<IRepository<Inventory, Guid>>();
            var warehouseRepo = serviceProvider.GetService<IRepository<Kira.LaconicInvoicing.Warehouse.Entities.Warehouse, Guid>>();
            var sourceWarehouse = await warehouseRepo.GetFirstAsync(w => w.Number == dto.SourceWarehouseNumber);
            var destWarehouse = await warehouseRepo.GetFirstAsync(w => w.Number == dto.DestWarehouseNumber);

            var sourceInventories = inventoryRepo.Query().Where(iv => iv.WarehouseId == sourceWarehouse.Id);
            var destInventories = inventoryRepo.Query().Where(iv => iv.WarehouseId == destWarehouse.Id);
            var updateInventories = new List<Inventory>();
            var addInventories = new List<Inventory>();

            var dt = DateTime.Now;
            foreach (var item in items)
            {
                if (sourceInventories.Any(iv => iv.Number == item.Number))
                {
                    var inventory = await sourceInventories.FirstOrDefaultAsync(iv => iv.Number == item.Number);
                    inventory.Amount -= item.Amount;
                    inventory.DateTime = dt;
                    updateInventories.Add(inventory);
                }
                else if(destInventories.Any(iv=>iv.Number == item.Number))
                {
                    var inventory = await destInventories.FirstOrDefaultAsync(iv => iv.Number == item.Number);
                    inventory.Amount += item.Amount;
                    inventory.DateTime = dt;
                    updateInventories.Add(inventory);
                }
                else
                {
                    var inventory = new Inventory()
                    {
                        Id = Guid.NewGuid(),
                        WarehouseId = destWarehouse.Id,
                        GoodsCategory = item.GoodsCategory,
                        Number = item.Number,
                        Amount = item.Amount,
                        DateTime = dt
                    };
                    
                    addInventories.Add(inventory);
                }
            }

            await _transferOrderRepo.InsertAsync(transferOrder);
            await _transferOrderItemRepo.InsertAsync(items);
            if (updateInventories.Count > 0)
                await inventoryRepo.UpdateAsync(updateInventories.ToArray());
            if (addInventories.Count > 0)
                await inventoryRepo.InsertAsync(addInventories.ToArray());
            return true;
        }

        public async Task<TransferOrderOutputDto> GetAsync(Guid id)
        {
            var transferOrder = (await _transferOrderRepo.GetAsync(id)).MapTo<TransferOrderOutputDto>();
            transferOrder.Items = (await _transferOrderItemRepo.Query().Where(p => p.TransferOrderId == id).ToListAsync())
             .Select(p => p.MapTo<TransferOrderItemOutputDto>())
             .ToArray();
            return transferOrder;
        }

        public async Task<bool> UpdateTransferOrderAsync(TransferOrderInputDto dto)
        {
            var transferOrder = dto.MapTo<TransferOrder>();
            await _transferOrderRepo.UpdateAsync(transferOrder);
            var items = dto.Items.Select(d => d.MapTo<TransferOrderItem>()).ToArray();
            await _transferOrderItemRepo.UpdateAsync(items);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _transferOrderRepo.DeleteAsync(ids);
            await _transferOrderItemRepo.DeleteBatchAsync(p => ids.Contains(p.TransferOrderId));
            return result.Successed;
        }
    }
}
