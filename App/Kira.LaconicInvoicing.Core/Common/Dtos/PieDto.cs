using OSharp.Entity;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    public class PieDto : IOutputDto
    {
        public string Name { get; set; }

        public double Ratio { get; set; }
    }
}