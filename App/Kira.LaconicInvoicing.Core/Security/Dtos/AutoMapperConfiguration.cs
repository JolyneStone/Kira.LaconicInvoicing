using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Security.Entities;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;
using OSharp.Json;


namespace Kira.LaconicInvoicing.Security.Dtos
{
    /// <summary>
    /// DTO对象映射类
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
            mapper.CreateMap<EntityRoleInputDto, EntityRole>()
                .ForMember(mr => mr.FilterGroupJson, opt => opt.MapFrom(dto => dto.FilterGroup.ToJsonString(false, false)));

            //mapper.CreateMap<EntityRole, EntityRoleOutputDto>()
            //    .ForMember(dto => dto.FilterGroup, opt => opt.ResolveUsing(mr => mr.FilterGroupJson?.FromJsonString<FilterGroup>()));
        }
    }
}