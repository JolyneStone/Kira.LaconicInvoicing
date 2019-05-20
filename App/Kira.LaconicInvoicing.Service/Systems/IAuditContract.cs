using System;
using System.Linq;
using System.Threading.Tasks;
using Kira.LaconicInvoicing.Systems.Entities;
using OSharp.Data;


namespace Kira.LaconicInvoicing.Service.Systems
{
    /// <summary>
    /// 业务契约：审计模块
    /// </summary>
    public interface IAuditContract
    {
        #region 操作审计信息业务

        /// <summary>
        /// 获取 操作审计信息查询数据集
        /// </summary>
        IQueryable<AuditOperation> AuditOperations { get; }

        /// <summary>
        /// 删除操作审计信息信息
        /// </summary>
        /// <param name="ids">要删除的操作审计信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteAuditOperations(params Guid[] ids);

        #endregion

        #region 数据审计信息业务

        /// <summary>
        /// 获取 数据审计信息查询数据集
        /// </summary>
        IQueryable<AuditEntity> AuditEntitys { get; }
        
        /// <summary>
        /// 获取 数据属性审计信息查询数据集
        /// </summary>
        IQueryable<AuditProperty> AuditProperties { get; }
        
        /// <summary>
        /// 删除数据审计信息信息
        /// </summary>
        /// <param name="ids">要删除的数据审计信息编号</param>
        /// <returns>业务操作结果</returns>
        Task<OperationResult> DeleteAuditEntitys(params Guid[] ids);

        #endregion
    }
}