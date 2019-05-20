using System;
using System.Collections.Generic;
using System.Security.Claims;
using OSharp.Collections;
using OSharp.Dependency;


namespace Kira.LaconicInvoicing.Service.Common
{
    /// <summary>
    /// 业务实现：通用业务
    /// </summary>
    public class CommonService : ICommonContract, IScopeDependency
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 初始化一个<see cref="CommonService"/>类型的新实例
        /// </summary>
        public CommonService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 测试测试
        /// </summary>
        public string Test()
        {
            List<object> list = new List<object>();

            ClaimsPrincipal user = _serviceProvider.GetCurrentUser();
            list.Add(user == null);
            list.Add(user?.GetType());
            list.Add(user?.Identity.Name);
            list.Add(user?.Identity.GetType());
            list.Add(user?.Identity.AuthenticationType);

            return list.ExpandAndToString("\r\n");
        }
    }
}