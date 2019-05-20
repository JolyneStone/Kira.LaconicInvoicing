using Microsoft.Extensions.Options;

namespace Kira.LaconicInvoicing.Infrastructure.Options
{
    public class ApplicationOptions : IOptions<ApplicationOptions>
    {
        ApplicationOptions IOptions<ApplicationOptions>.Value => this;

        ///// <summary>
        ///// 文件保存地址
        ///// </summary>
        //public string FrontendContentPath { get; set; }

        /// <summary>
        /// 头像配置
        /// </summary>
        public AvatarConfig Avatar { get; set; }
    }

    public sealed class AvatarConfig
    {
        /// <summary>
        /// 头像文件最大大小，请使用GetMaxAvatarLength方法进行调用
        /// </summary>
        public string MaxAvatarLength { get; set; }

        /// <summary>
        /// 默认头像
        /// </summary>
        public string DefaultAvatar { get; set; }
    }
}
