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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using OSharp.Mapping;
using Kira.LaconicInvoicing.Service.BaseData;

namespace Kira.LaconicInvoicing.Service.Sale
{
    public class CustomerService: ICustomerContract
    {
        private readonly IRepository<Customer, Guid> _customerRepo;
        private readonly ILogger<CustomerService> _logger;

        public CustomerService(IRepository<Customer, Guid> customerRepo,
              ILogger<CustomerService> logger)
        {
            _customerRepo = customerRepo;
            _logger = logger;
        }

        public async Task<PageData<CustomerOutputDto>> SearchAsync(QueryCondition<Customer> condition = null)
        {
            var query = _customerRepo.Query();
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

            return (await query.ToListAsync()).Select(v => v.MapTo<CustomerOutputDto>()).ToPageData(total);
        }

        public async Task<string> GetNewNumber(IServiceProvider serviceProvider)
        {
            var baseData = await serviceProvider.GetService<IBaseDataContract>()?.GetListAsync("NUMBERTYPE");
            var prefix = "";
            if (baseData == null || !baseData.TryGetValue("Customer", out prefix))
            {
                throw new BussinessException("无法找到客户的编号前缀");
            }

            var now = DateTime.Now;
            prefix = $"{now.Year}{now.Month.ToString().PadLeft(2, '0')}{now.Day.ToString().PadLeft(2, '0')}{prefix}";
            var maxNumber = await _customerRepo.Query().Where(v => v.Number.StartsWith(prefix)).MaxAsync(v => v.Number);
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

        public async Task<bool> AddCustomerAsync(CustomerInputDto dto)
        {
            var customer = dto.MapTo<Customer>();
            return await _customerRepo.InsertAsync(customer) > 0;
        }

        public async Task<CustomerOutputDto> GetAsync(Guid id)
        {
            return (await _customerRepo.GetAsync(id)).MapTo<CustomerOutputDto>();
        }

        public async Task<bool> UpdateCustomerAsync(CustomerInputDto dto)
        {
            var customer = dto.MapTo<Customer>();
            return await _customerRepo.UpdateAsync(customer) > 0;
        }

        public async Task<bool> DeleteAsync(Guid[] ids)
        {
            var result = await _customerRepo.DeleteAsync(ids);
            return result.Successed;
        }

        public async Task<PageData<ProductOutputDto>> GetProducts(QueryCondition<CustomerProduct> queryCondition, IServiceProvider serviceProvider)
        {
            var productProductQuery = serviceProvider.GetService<IRepository<CustomerProduct, Guid>>().Query();
            var productRepo = serviceProvider.GetService<IRepository<Product, Guid>>();
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
                           join m in productRepo.Query() on vm.ProductId equals m.Id
                           select m)
                          .ToListAsync())
                          .Select(m => m.MapTo<ProductOutputDto>()).ToPageData(total);
        }

        public async Task<bool> UpdateProducts(Guid id, Guid[] productIds, IServiceProvider serviceProvider)
        {
            var customerProductRepo = serviceProvider.GetService<IRepository<CustomerProduct, Guid>>();
            var sourceProductIds = await customerProductRepo.Query().Where(vm => vm.Id == id).Select(vm => vm.ProductId).ToListAsync();
            var addList = new List<Guid>();
            var deleteList = new List<Guid>();

            foreach (var vId in productIds)
            {
                if (!sourceProductIds.Contains(vId))
                {
                    addList.Add(vId);
                }
            }

            foreach (var vId in sourceProductIds)
            {
                if (!productIds.Contains(vId))
                {
                    deleteList.Add(vId);
                }
            }

            await customerProductRepo.DeleteBatchAsync(vm => vm.CustomerId == id && deleteList.Contains(vm.ProductId));
            await customerProductRepo.InsertAsync(addList.Select(i => new CustomerProduct() { Id = Guid.NewGuid(), CustomerId = id, ProductId = i }).ToArray());
            return true;
        }
    }
}
