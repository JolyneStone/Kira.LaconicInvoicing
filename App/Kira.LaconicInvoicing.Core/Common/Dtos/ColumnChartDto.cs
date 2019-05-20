using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    public class ColumnChartDto: IOutputDto
    {
        public List<ColumnDto> Data { get; set; }
    }
}
