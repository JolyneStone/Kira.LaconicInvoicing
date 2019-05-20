using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Identity.Dtos
{
    /// <summary>
    /// 发送重置邮件DTO
    /// </summary>
    public class SendResetMailDto
        {
            /// <summary>
            /// 获取或设置 NewEmail
            /// </summary>
            [Required]
            public string NewEmail { get; set; }

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
