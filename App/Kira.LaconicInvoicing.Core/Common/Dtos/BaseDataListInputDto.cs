using System.ComponentModel.DataAnnotations;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    public class BaseDataListInputDto
    {
        [Required]
        public string Type { get; set; }

        [Required]
        public BaseDataListDto BaseData { get; set; }
    }
}
