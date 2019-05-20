namespace Kira.LaconicInvoicing.Identity.Dtos
{
    /// <summary>
    /// 修改密码DTO
    /// </summary>
    public class ChangePasswordDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 获取或设置 旧密码
        /// </summary>
        public string OldPassword { get; set; }

        /// <summary>
        /// 获取或设置 新密码
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// 获取或设置 新密码确认
        /// </summary>
        public string ConfirmNewPassword { get; set; }
    }
}