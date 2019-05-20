using Kira.LaconicInvoicing.Service.Sale;
using Kira.LaconicInvoicing.Service.Warehouse;
using OSharp.AspNetCore;
using OSharp.Core;
using OSharp.Core.Packs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Kira.LaconicInvoicing.Web.Areas.Sale
{
    /// <summary>
    /// 销售模块
    /// </summary>
    [DependsOnPacks(typeof(SaleServicePack))]
    [Logined]
    [Description("销售模块")]
    public class SalePack : AspOsharpPack
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
