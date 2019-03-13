// -----------------------------------------------------------------------
//  <copyright file="AspOsharpPackManager.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-09 22:34</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OSharp.Core.Packs;
using OSharp.Exceptions;


namespace OSharp.AspNetCore
{
    /// <summary>
    /// AspNetCore ģ�������
    /// </summary>
    public class AspOsharpPackManager : OsharpPackManager, IAspUsePack
    {
        /// <summary>
        /// Ӧ��ģ����񣬽��ڷ�AspNetCore�����µ��ã�AspNetCore������ִ��<see cref="UsePack(IApplicationBuilder)"/>����
        /// </summary>
        /// <param name="provider">�����ṩ��</param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override void UsePack(IServiceProvider provider)
        {
            IHostingEnvironment environment = provider.GetService<IHostingEnvironment>();
            if (environment != null)
            {
                throw new OsharpException("��ǰ����AspNetCore��������ʹ��UsePack(IApplicationBuilder)���г�ʼ��");
            }

            base.UsePack(provider);
        }

        /// <summary>
        /// Ӧ��ģ����񣬽���AspNetCore�����µ��ã���AspNetCore������ִ��<see cref="UsePack(IServiceProvider)"/>����
        /// </summary>
        /// <param name="app">Ӧ�ó��򹹽���</param>
        public void UsePack(IApplicationBuilder app)
        {
            ILogger logger = app.ApplicationServices.GetLogger<AspOsharpPackManager>();
            logger.LogInformation("Osharp��ܳ�ʼ����ʼ");
            DateTime dtStart = DateTime.Now;

            foreach (OsharpPack pack in LoadedPacks)
            {
                pack.UsePack(app.ApplicationServices);

                if (pack is AspOsharpPack aspPack)
                {
                    aspPack.UsePack(app);
                }
            }

            TimeSpan ts = DateTime.Now.Subtract(dtStart);
            logger.LogInformation($"Osharp��ܳ�ʼ����ɣ���ʱ��{ts:g}");
        }
    }
}