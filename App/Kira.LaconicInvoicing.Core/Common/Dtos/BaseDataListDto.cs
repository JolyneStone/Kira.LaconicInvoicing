using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    /// <summary>
    /// 基础数据：列表DTO
    /// </summary>
    public class BaseDataListDto
    {
        /// <summary>
        /// 获取或设置 名称
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 获取或设置 代码
        /// </summary>
        [Required, StringLength(maximumLength: 10, MinimumLength = 1, ErrorMessage = "{0} 应在 {2}~{1} 个字符以内")]
        public string Code { get; set; }
    }
}
