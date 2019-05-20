using Kira.LaconicInvoicing.Service.Warehouse;
using OSharp.AspNetCore;
using OSharp.Core;
using OSharp.Core.Packs;
using System.ComponentModel;

namespace Kira.LaconicInvoicing.Web.Areas.Purchase
{
    /// <summary>
    /// 仓库模块
    /// </summary>
    [DependsOnPacks(typeof(WarehouseServicePack))]
    [Logined]
    [Description("仓库模块")]
    public class WarehousePack : AspOsharpPack
    {
        /// <summary>
        /// 获取 模块级别
        /// </summary>
        public override PackLevel Level => PackLevel.Business;

        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;
    }
}
