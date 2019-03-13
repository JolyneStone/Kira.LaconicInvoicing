// -----------------------------------------------------------------------
//  <copyright file="DbContextModelCache.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-12 14:14</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;

using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

using OSharp.Dependency;
using OSharp.Extensions;


namespace OSharp.Entity
{
    /// <summary>
    /// ����������ģ�ͻ���
    /// </summary>
    [Dependency(ServiceLifetime.Singleton, TryAdd = true, AddSelf = true)]
    public class DbContextModelCache
    {
        private readonly ConcurrentDictionary<Type, IModel> _dict = new ConcurrentDictionary<Type, IModel>();

        /// <summary>
        /// ��ȡָ�����������͵�ģ��
        /// </summary>
        /// <param name="dbContextType">����������</param>
        /// <returns>����ģ��</returns>
        public IModel Get(Type dbContextType)
        {
            return _dict.GetOrDefault(dbContextType);
        }

        /// <summary>
        /// ����ָ�����������͵�ģ��
        /// </summary>
        /// <param name="dbContextType">����������</param>
        /// <param name="model">ģ��</param>
        public void Set(Type dbContextType, IModel model)
        {
            _dict[dbContextType] = model;
        }

        /// <summary>
        /// �Ƴ�ָ�����������͵�ģ��
        /// </summary>
        public void Remove(Type dbContextType)
        {
            _dict.TryRemove(dbContextType, out IModel model);
        }
    }
}