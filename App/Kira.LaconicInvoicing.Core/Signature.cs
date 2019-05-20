using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kira.LaconicInvoicing
{
    // Token: 0x02000078 RID: 120
    internal class Signature : IEquatable<Signature>
    {
        // Token: 0x0600031C RID: 796 RVA: 0x0000B750 File Offset: 0x00009950
        public Signature(IEnumerable<DynamicProperty> properties)
        {
            this.properties = properties.ToArray<DynamicProperty>();
            this.hashCode = 0;
            foreach (DynamicProperty dynamicProperty in properties)
            {
                this.hashCode ^= (dynamicProperty.Name.GetHashCode() ^ dynamicProperty.Type.GetHashCode());
            }
        }

        // Token: 0x0600031D RID: 797 RVA: 0x0000B7D0 File Offset: 0x000099D0
        public override int GetHashCode()
        {
            return this.hashCode;
        }

        // Token: 0x0600031E RID: 798 RVA: 0x0000B7D8 File Offset: 0x000099D8
        public override bool Equals(object obj)
        {
            return obj is Signature && this.Equals((Signature)obj);
        }

        // Token: 0x0600031F RID: 799 RVA: 0x0000B7F0 File Offset: 0x000099F0
        public bool Equals(Signature other)
        {
            if (this.properties.Length != other.properties.Length)
            {
                return false;
            }
            for (int i = 0; i < this.properties.Length; i++)
            {
                if (this.properties[i].Name != other.properties[i].Name || this.properties[i].Type != other.properties[i].Type)
                {
                    return false;
                }
            }
            return true;
        }

        // Token: 0x0400010B RID: 267
        public DynamicProperty[] properties;

        // Token: 0x0400010C RID: 268
        public int hashCode;
    }
}
