﻿// -----------------------------------------------------------------------
//  <copyright file="JwtOptions.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-05-19 9:38</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Options
{
    /// <summary>
    /// JWT身份认证选项
    /// </summary>
    public class JwtOptions
    {
        /// <summary>
        /// 获取或设置 密钥
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// 获取或设置 发行方
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// 获取或设置 订阅方
        /// </summary>
        public string Audience{ get; set; }
    }
}