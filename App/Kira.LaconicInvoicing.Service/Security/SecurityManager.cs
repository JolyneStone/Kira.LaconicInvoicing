using System;
using Kira.LaconicInvoicing.Identity.Entities;
using Kira.LaconicInvoicing.Security.Dtos;
using Kira.LaconicInvoicing.Security.Entities;
using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Entity;
using OSharp.EventBuses;
using OSharp.Security;


namespace Kira.LaconicInvoicing.Service.Security
{
    /// <summary>
    /// 权限安全管理器
    /// </summary>
    public class SecurityManager
        : SecurityManagerBase<Function, FunctionInputDto, EntityInfo, EntityInfoInputDto,
            Module, ModuleInputDto, int, ModuleFunction, ModuleRole, ModuleUser, EntityRole, EntityRoleInputDto, UserRole, Role, int, User, int>
    {
        /// <summary>
        /// 初始化一个<see cref="SecurityManager"/>类型的新实例
        /// </summary>
        public SecurityManager(
            IEventBus eventBus,
            IRepository<Function, Guid> functionRepository,
            IRepository<EntityInfo, Guid> entityInfoRepository,
            IRepository<Module, int> moduleRepository,
            IRepository<ModuleFunction, Guid> moduleFunctionRepository,
            IRepository<ModuleRole, Guid> moduleRoleRepository,
            IRepository<ModuleUser, Guid> moduleUserRepository,
            IRepository<EntityRole, Guid> entityRoleRepository,
            IRepository<UserRole, Guid> userRoleRepository,
            IRepository<Role, int> roleRepository,
            IRepository<User, int> userRepository
        )
            : base(eventBus,
                functionRepository,
                entityInfoRepository,
                moduleRepository,
                moduleFunctionRepository,
                moduleRoleRepository,
                moduleUserRepository,
                entityRoleRepository,
                userRoleRepository,
                roleRepository,
                userRepository)
        { }
    }
}