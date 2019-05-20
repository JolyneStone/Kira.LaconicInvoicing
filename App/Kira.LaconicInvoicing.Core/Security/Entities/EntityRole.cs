using System.ComponentModel;
using Kira.LaconicInvoicing.Identity.Entities;
using OSharp.Core.EntityInfos;
using OSharp.Security;


namespace Kira.LaconicInvoicing.Security.Entities
{
    /// <summary>
    /// 实体：数据角色信息
    /// </summary>
    [Description("数据角色信息")]
    public class EntityRole : EntityRoleBase<int>
    {
        /// <summary>
        /// 获取或设置 所属角色信息
        /// </summary>
        public virtual Role Role { get; set; }

        /// <summary>
        /// 获取或设置 所属实体信息
        /// </summary>
        public virtual EntityInfo EntityInfo { get; set; }
    }
}