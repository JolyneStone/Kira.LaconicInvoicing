using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Sale.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;

namespace Kira.LaconicInvoicing.Sale.Dtos
{
    /// <summary>
    /// DTO对象映射配置
    /// </summary>
    [Dependency(ServiceLifetime.Singleton)]
    public class AutoMapperConfiguration : IAutoMapperConfiguration
    {
        /// <summary>
        /// 创建对象映射
        /// </summary>
        /// <param name="mapper">映射配置表达</param>
        public void CreateMaps(MapperConfigurationExpression mapper)
        {
            //mapper.CreateMap<SaleOrderInputDto, SaleOrderItem>().ForMember(po => po.SaleOrderId, opt => opt.Ignore());
            mapper.CreateMap<SaleOrder, SaleOrderOutputDto>().ForMember(po => po.Items, opt => opt.Ignore());
            mapper.CreateMap<SaleOrderOutputDto, SaleOrderPrintDataDto>()
              .ForMember(po => po.Items, opt => opt.MapFrom(po => po.Items))
              .ForMember(po => po.Status, opt => opt.MapFrom(po => po.Status.GetDisplayName()));
        }
    }
}