using System;
using System.Reflection;
using System.Text;

namespace Kira.LaconicInvoicing
{
    // Token: 0x02000074 RID: 116
    public abstract class DynamicClass : BaseObject
    {
        // Token: 0x0600030E RID: 782 RVA: 0x0000B574 File Offset: 0x00009774
        public override string ToString()
        {
            PropertyInfo[] properties = base.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public);
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("{");
            for (int i = 0; i < properties.Length; i++)
            {
                if (i > 0)
                {
                    stringBuilder.Append(", ");
                }
                stringBuilder.Append(properties[i].Name);
                stringBuilder.Append("=");
                stringBuilder.Append(properties[i].GetValue(this, null));
            }
            stringBuilder.Append("}");
            return stringBuilder.ToString();
        }
    }
}
