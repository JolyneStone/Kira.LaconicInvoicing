// -----------------------------------------------------------------------
//  <copyright file="DbContextBase.cs" company="OSharp开源团队">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2018-07-01 17:54</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using OSharp.Audits;
using OSharp.Core.Options;
using OSharp.Dependency;
using OSharp.EventBuses;


namespace OSharp.Entity
{
    /// <summary>
    /// EntityFramework上下文基类
    /// </summary>
    public abstract class DbContextBase : DbContext, IDbContext
    {
        private readonly ILogger _logger;
        private readonly OSharpDbContextOptions _osharpDbOptions;
        private readonly IEntityConfigurationTypeFinder _typeFinder;

        /// <summary>
        /// 初始化一个<see cref="DbContextBase"/>类型的新实例
        /// </summary>
        protected DbContextBase(DbContextOptions options, IEntityConfigurationTypeFinder typeFinder)
            : base(options)
        {
            _typeFinder = typeFinder;
            IOptions<OSharpOptions> osharpOptions = this.GetService<IOptions<OSharpOptions>>();
            _osharpDbOptions = osharpOptions?.Value.DbContexts.Values.FirstOrDefault(m => m.DbContextType == GetType());
            _logger = this.GetService<ILoggerFactory>().CreateLogger(GetType());
        }

        /// <summary>
        /// 获取或设置 所在上下文组
        /// </summary>
        public UnitOfWork UnitOfWork { get; set; }

        /// <summary>
        /// 开启或使用现有事务
        /// </summary>
        public void BeginOrUseTransaction()
        {
            if (UnitOfWork == null)
            {
                return;
            }
            UnitOfWork.BeginOrUseTransaction();
        }

        /// <summary>
        /// 异步开启或使用现有事务
        /// </summary>
        public async Task BeginOrUseTransactionAsync(CancellationToken cancellationToken)
        {
            if (UnitOfWork == null)
            {
                return;
            }
            await UnitOfWork.BeginOrUseTransactionAsync(cancellationToken);
        }

        /// <summary>
        /// 创建上下文数据模型时，对各个实体类的数据库映射细节进行配置
        /// </summary>
        /// <param name="modelBuilder">上下文数据模型构建器</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //通过实体配置信息将实体注册到当前上下文
            Type contextType = GetType();
            IEntityRegister[] registers = _typeFinder.GetEntityRegisters(contextType);
            foreach (IEntityRegister register in registers)
            {
                register.RegistTo(modelBuilder);
                _logger?.LogDebug($"将实体类“{register.EntityType}”注册到上下文“{contextType}”中");
            }
            _logger?.LogInformation($"上下文“{contextType}”注册了{registers.Length}个实体类");
        }

        /// <summary>
        /// 模型配置
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_osharpDbOptions != null && _osharpDbOptions.LazyLoadingProxiesEnabled)
            {
                optionsBuilder.UseLazyLoadingProxies();
            }
        }

        /// <summary>
        ///     将在此上下文中所做的所有更改保存到数据库中，同时自动开启事务或使用现有同连接事务
        /// </summary>
        /// <remarks>
        ///     此方法将自动调用 <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> 
        ///     若要在保存到基础数据库之前发现对实体实例的任何更改，请执行以下操作。这可以通过以下类型禁用
        ///     <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </remarks>
        /// <returns>
        ///     写入数据库的状态项的数目。
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     保存到数据库时遇到错误。
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     保存到数据库时会遇到并发冲突。
        ///     当在保存期间影响到意外数量的行时，就会发生并发冲突。
        ///     这通常是因为数据库中的数据在加载到内存后已经被修改。
        /// </exception>
        public override int SaveChanges()
        {
            IList<AuditEntityEntry> auditEntities = new List<AuditEntityEntry>();
            if (_osharpDbOptions != null && _osharpDbOptions.AuditEntityEnabled)
            {
                auditEntities = this.GetAuditEntities();
            }
            //开启或使用现有事务
            BeginOrUseTransaction();

            int count = base.SaveChanges();
            if (count > 0 && auditEntities.Count > 0)
            {
                AuditEntityEventData eventData = new AuditEntityEventData(auditEntities);
                IEventBus eventBus = this.GetService<IEventBus>();
                eventBus.Publish(this, eventData);
            }
            return count;
        }

        /// <summary>
        ///     异步地将此上下文中的所有更改保存到数据库中，同时自动开启事务或使用现有同连接事务
        /// </summary>
        /// <remarks>
        ///     <para>
        ///         此方法将自动调用 <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> 
        ///         若要在保存到基础数据库之前发现对实体实例的任何更改，请执行以下操作。这可以通过以下类型禁用
        ///         <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        ///     </para>
        ///     <para>
        ///         不支持同一上下文实例上的多个活动操作。请使用“等待”确保在此上下文上调用其他方法之前任何异步操作都已完成。
        ///     </para>
        /// </remarks>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        ///     表示异步保存操作的任务。任务结果包含写入数据库的状态条目数。
        /// </returns>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateException">
        ///     保存到数据库时遇到错误。
        /// </exception>
        /// <exception cref="T:Microsoft.EntityFrameworkCore.DbUpdateConcurrencyException">
        ///     保存到数据库时会遇到并发冲突。
        ///     当在保存期间影响到意外数量的行时，就会发生并发冲突。
        ///     这通常是因为数据库中的数据在加载到内存后已经被修改。
        /// </exception>
        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            IList<AuditEntityEntry> auditEntities = new List<AuditEntityEntry>();
            if (_osharpDbOptions != null && _osharpDbOptions.AuditEntityEnabled)
            {
                auditEntities = this.GetAuditEntities();
            }

            //开启或使用现有事务
            await BeginOrUseTransactionAsync(cancellationToken);

            int count = await base.SaveChangesAsync(cancellationToken);
            if (count > 0 && auditEntities.Count > 0)
            {
                AuditEntityEventData eventData = new AuditEntityEventData(auditEntities);
                IEventBus eventBus = this.GetService<IEventBus>();
                await eventBus.PublishAsync(this, eventData);
            }
            return count;
        }

        #region Overrides of DbContext

        /// <summary>
        ///     Releases the allocated resources for this context.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            UnitOfWork = null;
        }

        #endregion
    }
}