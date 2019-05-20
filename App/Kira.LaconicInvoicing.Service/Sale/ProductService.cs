using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using Kira.LaconicInvoicing.Service.BaseData;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using OSharp.Entity;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OSharp.Mapping;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public class ProductService:IProductContract
    {
        private readonly IRepository<Product, Guid> _productRepo;
        private readonly ILogger<ProductService> _logger;

        public ProductService(IRepository<Product, Guid> productRepo,
              ILogger<ProductService> logger)
        {
            _productRepo = productRepo;
            _logger = logger;
        }

        public async Task<PageData<ProductOutputDto>> SearchAsync(QueryCondition<Product> condition = null)
        {
            var query = _productRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<ProductOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("Product", out prefix))
            {
                throw new BussinessException("无法找到产品的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _productRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddProductAsync(ProductInputDto dto)
        {
            var product = dto.MapTo<Product>();
            return await _productRepo.InsertAsync(product) > 0;
        }

        public async Task<ProductOutputDto> GetAsync(Guid id)
        {
            return (await _productRepo.GetAsync(id)).MapTo<ProductOutputDto>();
        }

        public async Task<bool> UpdateProductAsync(ProductInputDto dto)
        {
            var product = dto.MapTo<Product>();
            return await _productRepo.UpdateAsync(product) > 0;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _productRepo.DeleteAsync(ids);
            return result.Successed;
        }

        public async Task<PageData<CustomerOutputDto>> GetCustomers(QueryCondition<CustomerProduct> queryCondition, IServiceProvider serviceProvider)
        {
            var productProductQuery = serviceProvider.GetService<IRepository<CustomerProduct, Guid>>().Query();
            var customerRepo = serviceProvider.GetService<IRepository<Customer, Guid>>();
            if (queryCondition.Filters != null && queryCondition.Filters.Count > 0)
            {
                productProductQuery = productProductQuery.Where(queryCondition.GetFilter(), queryCondition.GetValues());
            }

            if (queryCondition.Sorts != null && queryCondition.Sorts.Count > 0)
            {
                productProductQuery = productProductQuery.OrderBy(queryCondition.Sorts.ConvertToSortString());
            }

            var total = await productProductQuery.CountAsync();
            productProductQuery = productProductQuery.Skip((queryCondition.PageIndex - 1) * queryCondition.PageSize).Take(queryCondition.PageSize);

            return (await (from vm in productProductQuery
                           join v in customerRepo.Query() on vm.CustomerId equals v.Id
                           select v)
                          .ToListAsync())
                          .Select(v => v.MapTo<CustomerOutputDto>()).ToPageData(total);
        }

        public async Task<bool> UpdateCustomers(Guid id, Guid[] customersIds, IServiceProvider serviceProvider)
        {
            var customerProductRepo = serviceProvider.GetService<IRepository<CustomerProduct, Guid>>();
            var sourceProductIds = await customerProductRepo.Query().Where(vm => vm.Id == id).Select(vm => vm.CustomerId).ToListAsync();
            var addList = new List<Guid>();
            var deleteList = new List<Guid>();

            foreach (var vId in customersIds)
            {
                if (!sourceProductIds.Contains(vId))
                {
                    addList.Add(vId);
                }
            }

            foreach (var vId in sourceProductIds)
            {
                if (!customersIds.Contains(vId))
                {
                    deleteList.Add(vId);
                }
            }

            await customerProductRepo.DeleteBatchAsync(vm => vm.CustomerId == id && deleteList.Contains(vm.CustomerId));
            await customerProductRepo.InsertAsync(addList.Select(i => new CustomerProduct() { Id = Guid.NewGuid(), ProductId = id, CustomerId = i }).ToArray());
            return true;
        }
    }
}
