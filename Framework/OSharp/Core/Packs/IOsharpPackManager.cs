// -----------------------------------------------------------------------
//  <copyright file="IOsharpPackManager.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-10 0:12</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;


namespace OSharp.Core.Packs
{
    /// <summary>
    /// ����Osharpģ�������
    /// </summary>
    public interface IOsharpPackManager
    {
        /// <summary>
        /// ��ȡ �Զ�������������ģ����Ϣ
        /// </summary>
        IEnumerable<OsharpPack> SourcePacks { get; }

        /// <summary>
        /// ��ȡ ���ռ��ص�ģ����Ϣ����
        /// </summary>
        IEnumerable<OsharpPack> LoadedPacks { get; }

        /// <summary>
        /// ����ģ�����
        /// </summary>
        /// <param name="services">��������</param>
        /// <returns>��������</returns>
        IServiceCollection LoadPacks(IServiceCollection services);

        /// <summary>
        /// Ӧ��ģ�����
        /// </summary>
        /// <param name="provider">�����ṩ��</param>
        void UsePack(IServiceProvider provider);
    }
}