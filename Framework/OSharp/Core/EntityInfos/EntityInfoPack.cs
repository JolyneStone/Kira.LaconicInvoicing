﻿// -----------------------------------------------------------------------
//  <copyright file="EntityInfoPack.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-06-23 15:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.Extensions.DependencyInjection;

using OSharp.Core.Packs;


namespace OSharp.Core.EntityInfos
{
    /// <summary>
    /// 实体信息模块
    /// </summary>
    [Description("数据实体模块")]
    public class EntityInfoPack : OsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Application;

        /// <summary>
        /// 应用模块服务
        /// </summary>
        /// <param name="provider">服务提供者</param>
        public override void UsePack(IServiceProvider provider)
        {
            IEntityInfoHandler handler = provider.GetService<IEntityInfoHandler>();
            handler.Initialize();
            IsEnabled = true;
        }
    }
}