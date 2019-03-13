// -----------------------------------------------------------------------
//  <copyright file="IAspUsePack.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-10 0:31</last-date>
// -----------------------------------------------------------------------

using System;

using Microsoft.AspNetCore.Builder;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// ����AspNetCore�����µ�Ӧ��ģ����� 
    /// </summary>
    public interface IAspUsePack
    {
        /// <summary>
        /// Ӧ��ģ����񣬽���AspNetCore�����µ��ã���AspNetCore������ִ��<see cref="UsePack(IServiceProvider)"/>����
        /// </summary>
        /// <param name="app">Ӧ�ó��򹹽���</param>
        void UsePack(IApplicationBuilder app);
    }
}