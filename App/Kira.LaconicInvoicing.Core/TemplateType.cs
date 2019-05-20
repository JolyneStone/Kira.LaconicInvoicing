using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing
{
    public enum TemplateType
    {
        [EnumDisplayName("采购单")]
        PurchaseOrder = 0,
        [EnumDisplayName("销售单")]
        SaleOrder = 1,
        [EnumDisplayName("入库单")]
        InboundReceipt = 2,
        [EnumDisplayName("出库单")]
        OutboundReceipt = 3,
        [EnumDisplayName("调拨单")]
        TransferOrder = 4,
    }
}
