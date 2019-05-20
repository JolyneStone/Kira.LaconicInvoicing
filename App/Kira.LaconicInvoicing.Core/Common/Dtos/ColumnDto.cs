using OSharp.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    public class ColumnDto : IOutputDto
    {
        public string Xpos { get; set; }
        public double Ypos { get; set; }
    }
}
