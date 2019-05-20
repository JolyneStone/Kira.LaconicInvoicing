namespace Kira.LaconicInvoicing.Warehouse
{
    public enum WarehouseStatus
    {
        [EnumDisplayName("可用")]
        Usable = 0,
        [EnumDisplayName("满仓")]
        Full = 1,
        [EnumDisplayName("维护")]
        Maintain = 2,
        [EnumDisplayName("修建")]
        Build = 3,
        [EnumDisplayName("废弃")]
        Abandon = 4
    }
}
