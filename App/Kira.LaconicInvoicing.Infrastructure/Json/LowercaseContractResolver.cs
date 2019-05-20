using System;
using System.Collections.Generic;
using System.Text;

namespace Kira.LaconicInvoicing.Infrastructure.Json
{
    public class LowercaseContractResolver : Newtonsoft.Json.Serialization.DefaultContractResolver
    {
        protected override string ResolvePropertyName(string propertyName)
        {
            return propertyName.ToLower();
        }
    }
}
