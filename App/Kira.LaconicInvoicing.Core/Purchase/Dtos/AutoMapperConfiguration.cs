using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;

namespace Kira.LaconicInvoicing.Purchase.Dtos
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
            mapper.CreateMap<PurchaseOrderItemInputDto, PurchaseOrderItem>().ForMember(po => po.PurchaseOrderId, opt => opt.Ignore());
            //mapper.CreateMap<PurchaseOrderItem, PurchaseOrderItemOutputDto>();
            mapper.CreateMap<PurchaseOrder, PurchaseOrderOutputDto>().ForMember(po => po.Items, opt => opt.Ignore());
            mapper.CreateMap<PurchaseOrderOutputDto, PurchaseOrderPrintDataDto>()
                .ForMember(po => po.Items, opt => opt.MapFrom(po => po.Items))
                .ForMember(po => po.Status, opt => opt.MapFrom(po => po.Status.GetDisplayName()));
        }
    }
}