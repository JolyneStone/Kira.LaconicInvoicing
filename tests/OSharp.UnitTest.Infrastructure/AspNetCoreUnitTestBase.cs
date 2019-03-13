﻿using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.TestHost;


namespace OSharp.UnitTest.Infrastructure
{
    public abstract class AspNetCoreUnitTestBase<TStartup>
        where TStartup : class
    {
        protected AspNetCoreUnitTestBase()
        {
            var builder = CreateWebHostHuilder();
            Server = CreateTestServer(builder);

            Client = Server.CreateClient();

            ServerProvider = Server.Host.Services;
        }

        public TestServer Server { get; }

        public HttpClient Client { get; }

        public IServiceProvider ServerProvider { get; set; }

        protected virtual IWebHostBuilder CreateWebHostHuilder()
        {
            return new WebHostBuilder().UseStartup<TStartup>();
        }

        protected virtual TestServer CreateTestServer(IWebHostBuilder builder)
        {
            return new TestServer(builder);
        }
    }
}
