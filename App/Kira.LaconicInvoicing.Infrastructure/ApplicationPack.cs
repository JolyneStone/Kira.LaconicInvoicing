using Kira.LaconicInvoicing.Infrastructure.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using OSharp.AspNetCore;
using OSharp.Core.Packs;
using OSharp.Extensions;
using System;
using System.ComponentModel;

namespace Kira.LaconicInvoicing.Infrastructure
{
    /// <summary>
    /// Kira.LaconicInvoicing.Infrastructure模块
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    [Description("Kira.LaconicInvoicing.Infrastructure模块 ")]
    public class ApplicationPack: AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别，级别越小越先启动
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 0;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            IConfiguration configuration = services.GetConfiguration();
            //var frontendContents = configuration["Application:FrontendContents"];
            //if (frontendContents.IsNullOrEmpty())
            //{
            //    throw new Exception("配置文件中Application节点的FrontendContents不能为空");
            //}

            services.Configure<ApplicationOptions>(configuration.GetSection("Application"));

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            //IConfiguration configuration = app.ApplicationServices.GetService<IConfiguration>();
        }
    }
}
