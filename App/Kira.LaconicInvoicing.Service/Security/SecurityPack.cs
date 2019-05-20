using System.ComponentModel;
using Kira.LaconicInvoicing.Security.Dtos;
using Kira.LaconicInvoicing.Security.Entities;
using OSharp.Core.EntityInfos;
using OSharp.Core.Functions;
using OSharp.Security;
using OSharp.Secutiry;


namespace Kira.LaconicInvoicing.Service.Security
{
    /// <summary>
    /// 权限安全模块
    /// </summary>
    [Description("权限安全模块")]
    public class SecurityPack
        : SecurityPackBase<SecurityManager, FunctionAuthorization, FunctionAuthCache, DataAuthCache, ModuleHandler,
            Function, FunctionInputDto, EntityInfo, EntityInfoInputDto, Module, ModuleInputDto, int, ModuleFunction,
            ModuleRole, ModuleUser, EntityRole, EntityRoleInputDto, int, int>
    {
        /// <summary>
        /// 获取 模块启动顺序，模块启动的顺序先按级别启动，级别内部再按此顺序启动，
        /// 级别默认为0，表示无依赖，需要在同级别有依赖顺序的时候，再重写为>0的顺序值
        /// </summary>
        public override int Order => 1;
    }
}