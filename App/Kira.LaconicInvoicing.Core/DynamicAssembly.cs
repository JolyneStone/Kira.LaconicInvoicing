using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;

namespace Kira.LaconicInvoicing
{
    // Token: 0x02000030 RID: 48
    internal class DynamicAssembly
    {
        // Token: 0x17000039 RID: 57
        // (get) Token: 0x0600012C RID: 300 RVA: 0x000054B4 File Offset: 0x000036B4
        // (set) Token: 0x0600012D RID: 301 RVA: 0x000054BC File Offset: 0x000036BC
        public AssemblyBuilder Assembly { get; set; }

        // Token: 0x1700003A RID: 58
        // (get) Token: 0x0600012E RID: 302 RVA: 0x000054C5 File Offset: 0x000036C5
        // (set) Token: 0x0600012F RID: 303 RVA: 0x000054CD File Offset: 0x000036CD
        public ModuleBuilder Module { get; set; }

        // Token: 0x1700003B RID: 59
        // (get) Token: 0x06000130 RID: 304 RVA: 0x000054D6 File Offset: 0x000036D6
        // (set) Token: 0x06000131 RID: 305 RVA: 0x000054DE File Offset: 0x000036DE
        public List<TypeBuilder> Types { get; private set; }

        // Token: 0x06000132 RID: 306 RVA: 0x000054E7 File Offset: 0x000036E7
        public DynamicAssembly()
        {
            this.Types = new List<TypeBuilder>();
        }

        // Token: 0x06000133 RID: 307 RVA: 0x000054FC File Offset: 0x000036FC
        public TypeBuilder CreateDynamicType(string name, TypeAttributes attr, Type parent)
        {
            TypeBuilder typeBuilder = this.Module.DefineType(name, attr, parent);
            this.Types.Add(typeBuilder);
            return typeBuilder;
        }

        // Token: 0x06000134 RID: 308 RVA: 0x00005540 File Offset: 0x00003740
        public bool HasType(string typeName)
        {
            return this.Types.Any((TypeBuilder c) => c.Name == typeName);
        }
    }
}
