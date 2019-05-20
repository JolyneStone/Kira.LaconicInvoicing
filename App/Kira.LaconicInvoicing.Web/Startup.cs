using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Serialization;
using OSharp.AspNetCore;
using OSharp.AspNetCore.Mvc.Conventions;
using OSharp.AspNetCore.Mvc.Filters;
using OSharp.Core.Options;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Web
{
    public class Startup
    {
        IConfiguration Configuration { get; set; }

        public const string CorsName = "SignalRCors";

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(env.ContentRootPath)
                   .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                   .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)//增加环境配置文件，新建项目默认有
                   .AddEnvironmentVariables();
            Configuration = builder.Build();

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance); // 提高 中文支持 解决Aspose.Words异常问题
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // 配置SignalR
            services.AddSignalR(options =>
            {
                options.EnableDetailedErrors = true;
            });
            //.AddMessagePackProtocol();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsName,
                    policy => policy.AllowAnyOrigin()
                                    .AllowAnyHeader()
                                    .AllowAnyMethod()
                                    .AllowCredentials());
            });

            services.AddOSharp<AspOsharpPackManager>();

            // 覆盖OSharp框架的配置
            services.AddMvc(options =>
            {
                options.Conventions.Add(new DashedRoutingConvention());
                // 全局功能权限过滤器
                options.Filters.Add(new FunctionAuthorizationFilter());

                options.Filters.Add(new CorsAuthorizationFilterFactory(CorsName));
            }).AddJsonOptions(options =>
            {
                // 使用驼峰命名法
                options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/#/500");
                app.UseHsts().UseHttpsRedirection();
            }

            app.UseCors(CorsName);

            app.UseSignalR(routes =>
            {
                routes.MapHub<Service.Notification.NoticeHub>("/noticehub");
            });

            app.UseMiddleware<NodeNoFoundHandlerMiddleware>()
                .UseMiddleware<NodeExceptionHandlerMiddleware>()
                .UseDefaultFiles()
                .UseStaticFiles()
                .UseOSharp();
        }
    }
}