using Kira.LaconicInvoicing.Infrastructure.Options;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace App.Test.Options
{
    public class ApplicationOptionsTest
    {
        [Fact]
        public void InitOptionsTest()
        {
            var path = System.IO.Directory.GetCurrentDirectory();
            var builder = new ConfigurationBuilder()
                           .SetBasePath(@"C:\monster\Code\KiraNet\Kira.LaconicInvoicing\App\Kira.LaconicInvoicing.Web\")
                           .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                           .AddJsonFile($"appsettings.Development.json", optional: true);//增加环境配置文件，新建项目默认有
            var configuration = builder.Build();

            var serviceCollection = new ServiceCollection()
                .Configure<ApplicationOptions>(configuration.GetSection("Application"));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            var options = serviceProvider.GetService<IOptions<ApplicationOptions>>();

            Assert.NotNull(options);
            Assert.NotNull(options.Value);
            //Assert.NotEmpty(options.Value.FrontendContentPath);
            Assert.NotNull(options.Value.Avatar);
            Assert.NotEmpty(options.Value.Avatar.MaxAvatarLength);
            Assert.NotEmpty(options.Value.Avatar.DefaultAvatar);
        }

        [Fact]
        public void MaxAvatarLengthTest()
        {
            var applicationOptions = new ApplicationOptions()
            {
                Avatar = new AvatarConfig
                {
                    MaxAvatarLength = "10 * 1024 * 1024"
                }
            };

            var maxLength = applicationOptions.Avatar.GetMaxAvatarLength();
            Assert.Equal(maxLength, 10 * 1024 * 1024);
        }
    }
}
