using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    public class PieChartDto: IOutputDto
    {
        public List<PieDto> Data { get; set; } 
    }
}
