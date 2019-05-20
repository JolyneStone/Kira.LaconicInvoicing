using OSharp.Entity;

namespace Kira.LaconicInvoicing.Identity.Dtos
{
    /// <summary>
    /// 简单用户输出DTO
    /// </summary>
    public class UserOutputDto2 : IOutputDto
    {
        /// <summary>
        /// 获取或设置 用户编号
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 获取或设置 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 获取或设置 用户昵称
        /// </summary>
        public string NickName { get; set; }

        /// <summary>
        /// 获取或设置 用户邮箱
        /// </summary>
        public string Email { get; set; }
    }
}