﻿// -----------------------------------------------------------------------
//  <copyright file="IDbContextOptionsBuilderCreator.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2017 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2017-08-21 1:09</last-date>
// -----------------------------------------------------------------------

using System.Data.Common;

using Microsoft.EntityFrameworkCore;

using OSharp.Dependency;


namespace OSharp.Entity
{
    /// <summary>
    /// 定义<see cref="DbContextOptionsBuilder"/>创建器
    /// </summary>
    [MultipleDependency]
    public interface IDbContextOptionsBuilderCreator
    {
        /// <summary>
        /// 获取 数据库类型名称，如SqlServer，MySql，Sqlite等
        /// </summary>
        DatabaseType Type { get; }

        /// <summary>
        /// 创建<see cref="DbContextOptionsBuilder"/>对象
        /// </summary>
        /// <param name="connectionString">连接字符串</param>
        /// <param name="existingConnection">已存在的连接对象</param>
        /// <returns></returns>
        DbContextOptionsBuilder Create(string connectionString, DbConnection existingConnection);
    }
}