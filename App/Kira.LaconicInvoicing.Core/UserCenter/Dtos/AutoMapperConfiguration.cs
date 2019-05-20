using AutoMapper.Configuration;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.UserCenter.Dtos;
using Microsoft.Extensions.DependencyInjection;
using OSharp.AutoMapper;
using OSharp.Dependency;
using OSharp.Identity;

namespace Kiropt.LaconicInvoicing.UserCenter.Dtos
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
            // Usage Mapper.MapTo<UserDetail>(User).Map(UserDetail)
            mapper.CreateMap<User, UserDetailOutputDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(u => u.Id))
                .ForMember(d => d.Avatar, opt => opt.MapFrom(u => u.HeadImg));
            mapper.CreateMap<UserDetail, UserDetailOutputDto>()
                .ForMember(d => d.Profile, opt => opt.MapFrom(u => u.Profile));

            mapper.CreateMap<OnlineUser, UserDetailOutputDto>()
                .ForMember(d => d.UserId, opt => opt.MapFrom(u => u.Id))
                .ForMember(d => d.Avatar, opt => opt.MapFrom(u => u.HeadImg))
                .ForMember(d => d.Telephone, opt => opt.MapFrom(u => u.PhoneNumber));
        }
    }
}