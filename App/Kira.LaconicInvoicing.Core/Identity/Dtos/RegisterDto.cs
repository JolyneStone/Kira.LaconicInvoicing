using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Identity.Dtos
{
    /// <summary>
    /// 用户注册信息DTO
    /// </summary>
    public class RegisterDto
    {
        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        [Required, StringLength(30, MinimumLength = 3, ErrorMessage = "{0} 应在 {2}~{1} 个字符以内")]
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 密码
        /// </summary>
        [DataType(DataType.Password)]
        public string Password { get; set; }

        /// <summary>
        /// 获取或设置 确认密码
        /// </summary>
        [Compare("Password", ErrorMessage = "密码与确认密码不匹配")]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        [StringLength(20, MinimumLength = 3, ErrorMessage = "{0} 应在 {2}~{1} 个字符以内")]
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 电子邮箱
        /// </summary>
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        /// <summary>
        /// 获取或设置 注册IP
        /// </summary>
        public string RegisterIp { get; set; }

        /// <summary>
        /// 获取或设置 验证码
        /// </summary>
        public string VerifyCode { get; set; }

        /// <summary>
        /// 获取或设置 验证码编号
        /// </summary>
        public string VerifyCodeId { get; set; }
    }
}