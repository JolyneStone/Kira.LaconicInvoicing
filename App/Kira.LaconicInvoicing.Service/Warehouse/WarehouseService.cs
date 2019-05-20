using Kira.LaconicInvoicing.Service.BaseData;
using Kira.LaconicInvoicing.Warehouse.Dtos;
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
using WarehouseEntity = Kira.LaconicInvoicing.Warehouse.Entities.Warehouse;

namespace Kira.LaconicInvoicing.Service.Warehouse
{
    public class WarehouseService : IWarehouseContract
    {
        private readonly IRepository<WarehouseEntity, Guid> _materialRepo;
        private readonly ILogger<WarehouseService> _logger;

        public WarehouseService(IRepository<WarehouseEntity, Guid> materialRepo,
              ILogger<WarehouseService> logger)
        {
            _materialRepo = materialRepo;
            _logger = logger;
        }

        public async Task<PageData<WarehouseOutputDto>> SearchAsync(QueryCondition<WarehouseEntity> condition = null)
        {
            var query = _materialRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<WarehouseOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("Warehouse", out prefix))
            {
                throw new BussinessException("无法找到仓库的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _materialRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddWarehouseAsync(WarehouseInputDto dto)
        {
            var material = dto.MapTo<WarehouseEntity>();
            return await _materialRepo.InsertAsync(material) > 0;
        }

        public async Task<WarehouseOutputDto> GetAsync(Guid id)
        {
            return (await _materialRepo.GetAsync(id)).MapTo<WarehouseOutputDto>();
        }

        public async Task<bool> UpdateWarehouseAsync(WarehouseInputDto dto)
        {
            var material = dto.MapTo<WarehouseEntity>();
            return await _materialRepo.UpdateAsync(material) > 0;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _materialRepo.DeleteAsync(ids);
            return result.Successed;
        }
    }
}
