using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Kira.LaconicInvoicing;
using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using OSharp.Entity;
using OSharp.Filter;
using OSharp.Mapping;
using Kira.LaconicInvoicing.Service.BaseData;
using OSharp.Data;

namespace Kira.LaconicInvoicing.Service.Purcachase
{
    public class MaterialService : IMaterialContract
    {
        private readonly IRepository<Material, Guid> _materialRepo;
        private readonly ILogger<MaterialService> _logger;

        public MaterialService(IRepository<Material, Guid> materialRepo,
              ILogger<MaterialService> logger)
        {
            _materialRepo = materialRepo;
            _logger = logger;
        }

        public async Task<PageData<MaterialOutputDto>> SearchAsync(QueryCondition<Material> condition = null)
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

            return (await query.ToListAsync()).Select(v => v.MapTo<MaterialOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("Material", out prefix))
            {
                throw new BussinessException("无法找到原料的编号前缀");
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

        public async Task<bool> AddMaterialAsync(MaterialInputDto dto)
        {
            var material = dto.MapTo<Material>();
            return await _materialRepo.InsertAsync(material) > 0;
        }

        public async Task<MaterialOutputDto> GetAsync(Guid id)
        {
            return (await _materialRepo.GetAsync(id)).MapTo<MaterialOutputDto>();
        }

        public async Task<bool> UpdateMaterialAsync(MaterialInputDto dto)
        {
            var material = dto.MapTo<Material>();
            return await _materialRepo.UpdateAsync(material) > 0;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _materialRepo.DeleteAsync(ids);
            return result.Successed;
        }

        public async Task<PageData<VendorOutputDto>> GetVendors(QueryCondition<VendorMaterial> queryCondition, IServiceProvider serviceProvider)
        {
            var materialMaterialQuery = serviceProvider.GetService<IRepository<VendorMaterial, Guid>>().Query();
            var vendorRepo = serviceProvider.GetService<IRepository<Vendor, Guid>>();
            if (queryCondition.Filters != null && queryCondition.Filters.Count > 0)
            {
                materialMaterialQuery = materialMaterialQuery.Where(queryCondition.GetFilter(), queryCondition.GetValues());
            }

            if (queryCondition.Sorts != null && queryCondition.Sorts.Count > 0)
            {
                materialMaterialQuery = materialMaterialQuery.OrderBy(queryCondition.Sorts.ConvertToSortString());
            }

            var total = await materialMaterialQuery.CountAsync();
            materialMaterialQuery = materialMaterialQuery.Skip((queryCondition.PageIndex - 1) * queryCondition.PageSize).Take(queryCondition.PageSize);

            return (await (from vm in materialMaterialQuery
                          join v in vendorRepo.Query() on vm.VendorId equals v.Id
                          select v)
                          .ToListAsync())
                          .Select(v=>v.MapTo<VendorOutputDto>()).ToPageData(total);
        }

        public async Task<bool> UpdateVendors(Guid id, Guid[] vendorsIds, IServiceProvider serviceProvider)
        {
            var vendorMaterialRepo = serviceProvider.GetService<IRepository<VendorMaterial, Guid>>();
            var sourceMaterialIds = await vendorMaterialRepo.Query().Where(vm => vm.Id == id).Select(vm => vm.VendorId).ToListAsync();
            var addList = new List<Guid>();
            var deleteList = new List<Guid>();

            foreach (var vId in vendorsIds)
            {
                if (!sourceMaterialIds.Contains(vId))
                {
                    addList.Add(vId);
                }
            }

            foreach (var vId in sourceMaterialIds)
            {
                if (!vendorsIds.Contains(vId))
                {
                    deleteList.Add(vId);
                }
            }

            await vendorMaterialRepo.DeleteBatchAsync(vm => vm.VendorId == id && deleteList.Contains(vm.VendorId));
            await vendorMaterialRepo.InsertAsync(addList.Select(i => new VendorMaterial() { Id = Guid.NewGuid(), MaterialId = id, VendorId = i }).ToArray());
            return true;
        }
    }
}
