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
    public class VendorService : IVendorContract
    {
        private readonly IRepository<Vendor, Guid> _vendorRepo;
        private readonly ILogger<VendorService> _logger;

        public VendorService(IRepository<Vendor, Guid> vendorRepo,
              ILogger<VendorService> logger)
        {
            _vendorRepo = vendorRepo;
            _logger = logger;
        }

        public async Task<PageData<VendorOutputDto>> SearchAsync(QueryCondition<Vendor> condition = null)
        {
            var query = _vendorRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<VendorOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("Vendor", out prefix))
            {
                throw new BussinessException("无法找到供应商的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _vendorRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddVendorAsync(VendorInputDto dto)
        {
            var vendor = dto.MapTo<Vendor>();
            return await _vendorRepo.InsertAsync(vendor) > 0;
        }

        public async Task<VendorOutputDto> GetAsync(Guid id)
        {
            return (await _vendorRepo.GetAsync(id)).MapTo<VendorOutputDto>();
        }

        public async Task<bool> UpdateVendorAsync(VendorInputDto dto)
        {
            var vendor = dto.MapTo<Vendor>();
            return await _vendorRepo.UpdateAsync(vendor) > 0;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _vendorRepo.DeleteAsync(ids);
            return result.Successed;
        }

        public async Task<PageData<MaterialOutputDto>> GetMaterials(QueryCondition<VendorMaterial> queryCondition, IServiceProvider serviceProvider)
        {
            var materialMaterialQuery = serviceProvider.GetService<IRepository<VendorMaterial, Guid>>().Query();
            var materialRepo = serviceProvider.GetService<IRepository<Material, Guid>>();
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
                           join m in materialRepo.Query() on vm.MaterialId equals m.Id
                           select m)
                          .ToListAsync())
                          .Select(m => m.MapTo<MaterialOutputDto>()).ToPageData(total);
        }

        public async Task<bool> UpdateMaterials(Guid id, Guid[] materialIds, IServiceProvider serviceProvider)
        {
            var vendorMaterialRepo = serviceProvider.GetService<IRepository<VendorMaterial, Guid>>();
            var sourceMaterialIds = await vendorMaterialRepo.Query().Where(vm => vm.Id == id).Select(vm => vm.MaterialId).ToListAsync();
            var addList = new List<Guid>();
            var deleteList = new List<Guid>();

            foreach (var vId in materialIds)
            {
                if (!sourceMaterialIds.Contains(vId))
                {
                    addList.Add(vId);
                }
            }

            foreach (var vId in sourceMaterialIds)
            {
                if (!materialIds.Contains(vId))
                {
                    deleteList.Add(vId);
                }
            }

            await vendorMaterialRepo.DeleteBatchAsync(vm => vm.VendorId == id && deleteList.Contains(vm.MaterialId));
            await vendorMaterialRepo.InsertAsync(addList.Select(i => new VendorMaterial() { Id = Guid.NewGuid(), VendorId = id, MaterialId = i }).ToArray());
            return true;
        }
    }
}
