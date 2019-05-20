using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Purchase.Dtos;
using Kira.LaconicInvoicing.Purchase.Entities;
using Kira.LaconicInvoicing.Sale.Entities;
using Kira.LaconicInvoicing.Warehouse.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;

namespace Kira.LaconicInvoicing.Warehouse.Dtos
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
            //mapper.CreateMap<InboundReceiptItemInputDto, InboundReceiptItem>().ForMember(ib => ib.InboundReceiptId, opt => opt.Ignore());
            mapper.CreateMap<InboundReceipt, InboundReceiptOutputDto>().ForMember(ib => ib.Items, opt => opt.Ignore());

            //mapper.CreateMap<OutboundReceiptItemInputDto, OutboundReceiptItem>().ForMember(ob => ob.OutboundReceiptId, opt => opt.Ignore());
            mapper.CreateMap<OutboundReceipt, OutboundReceiptOutputDto>().ForMember(ob => ob.Items, opt => opt.Ignore());

            mapper.CreateMap<TransferOrder, TransferOrderOutputDto>().ForMember(to => to.Items, opt => opt.Ignore());

            mapper.CreateMap<Inventory, InventoryOutputDto>();
            mapper.CreateMap<Material, InventoryOutputDto>()
                .ForMember(d => d.Id, opt => opt.Ignore())
                .ForMember(d => d.Number, opt => opt.Ignore())
                .ForMember(d => d.Name, opt => opt.Ignore());

            mapper.CreateMap<Product, InventoryOutputDto>()
              .ForMember(d => d.Id, opt => opt.Ignore())
              .ForMember(d => d.Number, opt => opt.Ignore())
              .ForMember(d => d.Name, opt => opt.Ignore())
              .ForMember(d => d.Price, opt => opt.MapFrom(p => p.CostPrice));

            mapper.CreateMap<InboundReceiptOutputDto, InboundReceiptPrintDataDto>()
                .ForMember(po => po.Items, opt => opt.MapFrom(po => po.Items));

            mapper.CreateMap<OutboundReceiptOutputDto, OutboundReceiptPrintDataDto>()
                .ForMember(po => po.Items, opt => opt.MapFrom(po => po.Items));

            mapper.CreateMap<TransferOrderOutputDto, TransferOrderPrintDataDto>()
                .ForMember(po => po.Items, opt => opt.MapFrom(po => po.Items));
        }
    }
}