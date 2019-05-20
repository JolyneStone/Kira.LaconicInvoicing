namespace Kira.LaconicInvoicing.Sale
{
    public enum SaleOrderStatus
    {
        [EnumDisplayName("预订")]
        Book = 0,
        [EnumDisplayName("下单")]
        Order = 1,
        [EnumDisplayName("运输中")]
        Transporting = 2,
        [EnumDisplayName("收货")]
        Receipt = 3,
        [EnumDisplayName("完成")]
        Complete = 4,
        [EnumDisplayName("退货")]
        Return = 9
    }
}
