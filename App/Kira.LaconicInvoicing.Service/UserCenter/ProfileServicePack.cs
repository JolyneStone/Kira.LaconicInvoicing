using Kira.LaconicInvoicing.Infrastructure;
using Kira.LaconicInvoicing.Service.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.AspNetCore;
using OSharp.Core.Packs;
using System.ComponentModel;

namespace Kira.LaconicInvoicing.Service.UserCenter
{
    /// <summary>
    /// Kira.LaconicInvoicing.ProfileServicePack模块
    /// </summary>
    [DependsOnPacks(typeof(ApplicationPack))]
    [Description("Kira.LaconicInvoicing.ProfileServicePack模块")]
    public class ProfileServicePack: AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Business;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.TryAddScoped<IUserCenterContract, UserCenterService>();
            return services;
        }
    }
}
