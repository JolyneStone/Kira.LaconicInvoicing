using System;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Security.Entities;
using OSharp.Core.EntityInfos;
using OSharp.Security;


namespace Kira.LaconicInvoicing.Service.Security
{
    /// <summary>
    /// 数据权限缓存
    /// </summary>
    public class DataAuthCache : DataAuthCacheBase<EntityRole, Role, EntityInfo, int>
    {
        /// <summary>
        /// 初始化一个<see cref="DataAuthCacheBase{TEntityRole, TRole, TEntityInfo, TRoleKey}"/>类型的新实例
        /// </summary>
        public DataAuthCache(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}