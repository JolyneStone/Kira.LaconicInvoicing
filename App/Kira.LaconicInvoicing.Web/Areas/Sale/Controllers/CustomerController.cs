using Kira.LaconicInvoicing.Common.Dtos;
using Kira.LaconicInvoicing.Sale.Dtos;
using Kira.LaconicInvoicing.Sale.Entities;
using Kira.LaconicInvoicing.Service.Purcachase;
using Kira.LaconicInvoicing.Service.Sale;
using Microsoft.AspNetCore.Mvc;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.AspNetCore.UI;
using OSharp.Core.Modules;
using OSharp.Data;
using OSharp.Entity;
using OSharp.Filter;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Sale.Controllers
{
    [ModuleInfo(Order = 1, Position = "Sale", PositionName = "采购模块")]
    [Description("管理-客户信息")]
    public class CustomerController : SaleApiController
    {
        private readonly ICustomerContract _customerContract;

        /// <summary>
        /// 初始化一个<see cref="CustomerController"/>类型的新实例
        /// </summary>
        public CustomerController(ICustomerContract customerContract)
        {
            _customerContract = customerContract;
        }

        /// <summary>
        /// 获取新客户编码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("获取新客户编码")]
        public async Task<AjaxResult> GetNewNumber()
        {
            return await AjaxResult.Business(async () =>
            {
                var data = await _customerContract.GetNewNumber(ServiceProvider);
                return AjaxResult.CreateSuccess(data);
            });
        }

        /// <summary>
        /// 查询客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("查询客户信息")]
        public async Task<AjaxResult> Search([FromBody]QueryContainer query = null)
        {
            return await AjaxResult.Business(async () =>
             {
                 var data = await _customerContract.SearchAsync(query?.ToQueryCondition<Customer>());
                 return AjaxResult.CreateSuccess(data);
             });
        }

        /// <summary>
        /// 添加客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [ModuleInfo]
        [Description("添加客户信息")]
        public async Task<AjaxResult> Add([FromBody]CustomerInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                dto.Id = Guid.NewGuid();
                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                await _customerContract.AddCustomerAsync(dto);
                result.Type = AjaxResultType.Success;
            });
        }

        /// <summary>
        /// 根据指定Id获取客户信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ModuleInfo]
        [Description("根据指定Id获取客户信息")]
        public async Task<AjaxResult> Get(Guid id)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(id, nameof(id));
                Check.NotEmpty(id, nameof(id));

                var dto = await _customerContract.GetAsync(id);
                if (dto == null)
                {
                    result.Error("找不到指定的客户信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 更新客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新客户信息")]
        public async Task<AjaxResult> Update([FromBody]CustomerInputDto dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));

                if (!ModelState.IsValid)
                {
                    result.Error("提交信息验证失败");
                    return;
                }
                if (String.IsNullOrWhiteSpace(dto.Operator))
                    dto.Operator = User.Identity.Name;
                dto.DateTime = DateTime.Now;
                await _customerContract.UpdateCustomerAsync(dto);
                result.Type = AjaxResultType.Success;
                if (dto == null)
                {
                    result.Error("找不到指定的客户信息");
                }
                else
                {
                    result.Success(dto);
                }
            });
        }

        /// <summary>
        /// 删除指定Id客户信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [HttpDelete]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("删除指定Id客户信息")]
        public async Task<AjaxResult> Delete(Guid[] ids)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(ids, nameof(ids));

                var r = await _customerContract.DeleteAsync(ids);
                if (r == false)
                {
                    result.Error("无法删除指定的客户信息");
                }
                else
                {
                    result.Success();
                }
            });
        }

        /// <summary>
        /// 获取指定客户的产品
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [Description("获取指定客户的产品")]
        public async Task<AjaxResult> GetProducts([FromBody]QueryContainer query)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(query, nameof(query));
                var queryCondition = query.ToQueryCondition<CustomerProduct>();
                foreach (var f in queryCondition.Filters)
                {
                    if (!string.IsNullOrWhiteSpace(f.Field) && f.Field.EqualsIgnoreCase("CustomerId"))
                    {
                        f.Value = Guid.Parse(f.Value.ToString());
                    }
                }
                var data = await _customerContract.GetProducts(queryCondition, ServiceProvider);
                result.Success(data);
            });
        }

        /// <summary>
        /// 更新指定客户的产品信息
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [ModuleInfo]
        [ServiceFilter(typeof(UnitOfWorkAttribute))]
        [Description("更新指定客户的产品信息")]
        public async Task<AjaxResult> UpdateProducts([FromBody]RelevantDto<Guid, Guid> dto)
        {
            return await AjaxResult.Business(async result =>
            {
                Check.NotNull(dto, nameof(dto));
                Check.NotNull(dto.Id, nameof(dto.Id));
                Check.NotNull(dto.Ids, nameof(dto.Ids));

                dto.Ids = dto.Ids.Distinct().ToArray();
                var r = await _customerContract.UpdateProducts(dto.Id, dto.Ids, ServiceProvider);
                if (r == false)
                {
                    result.Error("无法更新指定客户的产品信息");
                }
                else
                {
                    result.Success();
                }
            });
        }
    }
}
