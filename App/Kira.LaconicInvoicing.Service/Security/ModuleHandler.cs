using System;
using Kira.LaconicInvoicing.Security.Dtos;
using Kira.LaconicInvoicing.Security.Entities;
using OSharp.Security;


namespace Kira.LaconicInvoicing.Service.Security
{
    /// <summary>
    /// 模块信息处理器
    /// </summary>
    public class ModuleHandler : ModuleHandlerBase<Module, ModuleInputDto, int, ModuleFunction>
    {
        /// <summary>
        /// 初始化一个<see cref="ModuleHandlerBase{TModule, TModuleInputDto, TModuleKey, TModuleFunction}"/>类型的新实例
        /// </summary>
        public ModuleHandler(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }
    }
}