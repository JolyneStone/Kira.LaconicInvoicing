﻿// -----------------------------------------------------------------------
//  <copyright file="MvcFunctionPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:26</last-date>
// -----------------------------------------------------------------------

using System.ComponentModel;
using System.Linq;

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

using OSharp.Core.Functions;
using OSharp.Core.Modules;
using OSharp.Core.Packs;
using OSharp.Dependency;


namespace OSharp.AspNetCore.Mvc
{
    /// <summary>
    /// MVC功能点模块
    /// </summary>
    [DependsOnPacks(typeof(AspNetCorePack))]
    [Description("MVC功能点模块")]
    public class MvcFunctionPack : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 将模块服务添加到依赖注入服务容器中
        /// </summary>
        /// <param name="services">依赖注入服务容器</param>
        /// <returns></returns>
        public override IServiceCollection AddServices(IServiceCollection services)
        {
            services.GetOrAddTypeFinder<IFunctionTypeFinder>(assemblyFinder => new MvcControllerTypeFinder(assemblyFinder));
            services.AddSingleton<IFunctionHandler, MvcFunctionHandler>();
            services.TryAddSingleton<IModuleInfoPicker, MvcModuleInfoPicker>();

            return services;
        }

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="app">应用程序构建器</param>
        public override void UsePack(IApplicationBuilder app)
        {
            IFunctionHandler functionHandler = app.ApplicationServices.GetServices<IFunctionHandler>().FirstOrDefault(m => m.GetType() == typeof(MvcFunctionHandler));
            if (functionHandler == null)
            {
                return;
            }
            functionHandler.Initialize();

            IsEnabled = true;
        }
    }
}