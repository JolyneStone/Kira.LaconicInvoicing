//using Microsoft.Extensions.Caching.Distributed;
//using Microsoft.Extensions.DependencyInjection;
//using OSharp.Identity;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Kira.LaconicInvoicing.Infrastructure.Cache
//{
//    public class OnlineUserDetailCache<TUserDetail, TUserDetailKey, TUser, TUserKey> : IOnlineUserDetailCache
//        where TUserDetail : UserBase<TUserDetailKey>
//        where TUserDetailKey : IEquatable<TUserDetailKey>
//        where TUser : RoleBase<TUserKey>
//        where TUserKey : IEquatable<TUserKey>
//    {
//        private readonly IServiceProvider _serviceProvider;
//        private readonly IOnlineUserCache _onlineUserCache;
//        private readonly IDistributedCache _cache;

//        public OnlineUserDetailCache(IServiceProvider serviceProvider)
//        {
//            _serviceProvider = serviceProvider;
//            _cache = serviceProvider.GetService<IDistributedCache>();
//            _onlineUserCache = serviceProvider.GetService<IOnlineUserCache>();
//        }

//        public OnlineUserDetail GetOrRefresh(string userName)
//        {
//            throw new NotImplementedException();
//        }

//        public Task<OnlineUserDetail> GetOrRefreshAsync(string userName)
//        {
//            throw new NotImplementedException();
//        }

//        public void Remove(params string[] userNames)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
