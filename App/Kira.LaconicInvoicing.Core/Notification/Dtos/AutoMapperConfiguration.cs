using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Notification.Entities;
using Kira.LaconicInvoicing.Purchase.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;

namespace Kira.LaconicInvoicing.Notification.Dtos
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
            mapper.CreateMap<Notice, NoticeOutputDto>().ForMember(n => n.IsRead, opt => opt.Ignore());
        }
    }
}