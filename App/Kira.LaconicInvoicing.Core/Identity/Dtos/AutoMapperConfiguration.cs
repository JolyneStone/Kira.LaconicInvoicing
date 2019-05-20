using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Identity.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;

namespace Kira.LaconicInvoicing.Identity.Dtos
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
            mapper.CreateMap<Role, RoleNode>().ForMember(rn => rn.RoleId, opt => opt.MapFrom(r => r.Id))
                .ForMember(rn => rn.RoleName, opt => opt.MapFrom(r => r.Name));
        }
    }
}