using System;
using System.Data;

namespace Kira.LaconicInvoicing.Infrastructure.Options
{
    public static class ApplicationOptionExtensions
    {
        /// <summary>
        /// 获取MaxAvatarLength的计算值
        /// </summary>
        /// <param name="this"></param>
        /// <returns></returns>
        public static int GetMaxAvatarLength(this AvatarConfig @this)
        {
            if (@this == null || string.IsNullOrWhiteSpace(@this.MaxAvatarLength))
                return 0;

            var val = Convert.ToInt32(new DataTable().Compute(@this.MaxAvatarLength.Trim(), null));
            return val;
        }
    }
}
