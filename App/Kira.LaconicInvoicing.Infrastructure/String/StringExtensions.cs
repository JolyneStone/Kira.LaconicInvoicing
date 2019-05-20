using System.IO;

namespace Kira.LaconicInvoicing.Infrastructure.String
{
    public static class StringExtensions
    {
        public static string ConvertToFrontendPath(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
                return "";

            return @this.Replace(@"\\", "/")
                        .Replace(@"\", "/");
        }

        public static string ConvertToServicePath(this string @this)
        {
            if (string.IsNullOrWhiteSpace(@this))
                return "";

            return @this.Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
