// -----------------------------------------------------------------------
using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Identity.Dtos
{
    /// <summary>
    /// 发送邮件DTO
    /// </summary>
    public class SendMailDto
    {
        /// <summary>
        /// 获取或设置 Email
        /// </summary>
        [Required]
        public string Email { get; set; }

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