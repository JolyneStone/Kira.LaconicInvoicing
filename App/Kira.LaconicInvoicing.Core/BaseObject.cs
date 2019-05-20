using System;
using System.Collections;
using System.Reflection;
using System.Runtime.Serialization;

namespace Kira.LaconicInvoicing
{
    [Serializable]
    public abstract class BaseObject : IBaseObject, ICloneable, IDisposable
    {
        // Token: 0x0600003D RID: 61 RVA: 0x000029B4 File Offset: 0x00000BB4
        public static object TryClone(object obj)
        {
            ICloneable cloneable = obj as ICloneable;
            if (cloneable != null)
            {
                return cloneable.Clone();
            }
            IList list = obj as IList;
            if (list != null)
            {
                return list.Clone();
            }
            return obj;
        }

        // Token: 0x0600003E RID: 62 RVA: 0x000029E4 File Offset: 0x00000BE4
        public virtual object Clone()
        {
            object obj = base.MemberwiseClone();
            obj.TraversalPropertiesInfo(new Func<PropertyInfo, object, object, bool>(this.ClonePropertyHandler), obj);
            return obj;
        }

        // Token: 0x0600003F RID: 63 RVA: 0x00002A0C File Offset: 0x00000C0C
        public virtual void Dispose()
        {
        }

        // Token: 0x06000040 RID: 64 RVA: 0x00002A0E File Offset: 0x00000C0E
        private bool ClonePropertyHandler(PropertyInfo pi, object value, object target)
        {
            if (!pi.CanWrite)
            {
                return true;
            }
            pi.SetValue(target, BaseObject.TryClone(value), null);
            return true;
        }

        // Token: 0x06000041 RID: 65 RVA: 0x00002A2C File Offset: 0x00000C2C
        private bool DisposePropertyHandler(string name, object value)
        {
            IDisposable disposable = value as IDisposable;
            if (disposable == null)
            {
                return true;
            }
            disposable.Dispose();
            return true;
        }
    }
}
