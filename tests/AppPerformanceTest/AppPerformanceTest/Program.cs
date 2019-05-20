using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Running;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace AppPerformanceTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Summary summary = BenchmarkRunner.Run<BenchmarkTest>();

            Console.ReadLine();
        }
    }

    [CoreJob]
    [MemoryDiagnoser]
    [RPlotExporter, RankColumn]
    //[DryJob]
    [JsonExporterAttribute.Brief]
    [JsonExporterAttribute.Full]
    [JsonExporterAttribute.BriefCompressed]
    [JsonExporterAttribute.FullCompressed]
    [JsonExporter("-custom", indentJson: true, excludeMeasurements: true)]
    public class BenchmarkTest
    {
        private static IHttpClientFactory _httpClientFactory;

        static BenchmarkTest()
        {
            IServiceCollection services = new ServiceCollection();
            //注入
            services.AddHttpClient("Test", c =>
            {
                c.BaseAddress = new Uri("http://localhost:7001/");
                //c.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
                c.DefaultRequestHeaders.Add("User-Agent", "HttpClientFactory-Sample");
            });
            //构建容器
            var serviceProvider = services.BuildServiceProvider();
            _httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();
        }

        public BenchmarkTest()
        {
        }

        [Benchmark]
        public void Get()
        {
            var client = _httpClientFactory.CreateClient("Test");
            client.GetAsync("/api/Test3/Get");
        }

        [Benchmark]
        public void Add()
        {
            var client = _httpClientFactory.CreateClient("Test");
            client.GetAsync("/api/Test3/Add");
        }

        [Benchmark]
        public void Update()
        {
            var client = _httpClientFactory.CreateClient("Test");
            client.GetAsync("/api/Test3/Update");
        }

        [Benchmark]
        public void Delete()
        {
            var client = _httpClientFactory.CreateClient("Test");
            client.GetAsync("/api/Test3/Delete");
        }
    }
}
