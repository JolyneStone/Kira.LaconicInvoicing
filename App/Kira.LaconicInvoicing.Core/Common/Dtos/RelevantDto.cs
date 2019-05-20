using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Common.Dtos
{
    public class RelevantDto<TKEY1, TKEY2>
    {
        public TKEY1 Id { get; set; }
        public TKEY2[] Ids { get; set; }
    }
}
