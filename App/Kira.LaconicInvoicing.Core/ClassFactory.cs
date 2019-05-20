using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using System.Security.Cryptography.Xml;
using System.Threading;

namespace Kira.LaconicInvoicing
{
    // Token: 0x02000079 RID: 121
    internal class ClassFactory
    {
        // Token: 0x06000321 RID: 801 RVA: 0x0000B878 File Offset: 0x00009A78
        private ClassFactory()
        {
            AssemblyName name = new AssemblyName("DynamicClasses");
            AssemblyBuilder assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(name, AssemblyBuilderAccess.Run);
            this.module = assemblyBuilder.DefineDynamicModule("Module");
            this.classes = new Dictionary<Signature, Type>();
            this.rwLock = new ReaderWriterLock();
        }

        // Token: 0x06000322 RID: 802 RVA: 0x0000B8CC File Offset: 0x00009ACC
        public Type GetDynamicClass(IEnumerable<DynamicProperty> properties)
        {
            Type result;
            lock (this.classes)
            {
                Signature signature = new Signature(properties);
                Type type;
                if (!this.classes.TryGetValue(signature, out type))
                {
                    type = this.CreateDynamicClass(signature.properties);
                    this.classes.Add(signature, type);
                }
                result = type;
            }
            return result;
        }

        // Token: 0x06000323 RID: 803 RVA: 0x0000B93C File Offset: 0x00009B3C
        private Type CreateDynamicClass(DynamicProperty[] properties)
        {
            LockCookie lockCookie = this.rwLock.UpgradeToWriterLock(-1);
            Type result;
            try
            {
                string name = "DynamicClass" + (this.classCount + 1);
                TypeBuilder typeBuilder = this.module.DefineType(name, TypeAttributes.Public, typeof(DynamicClass));
                FieldInfo[] fields = this.GenerateProperties(typeBuilder, properties);
                this.GenerateEquals(typeBuilder, fields);
                this.GenerateGetHashCode(typeBuilder, fields);
                Type type = typeBuilder.CreateTypeInfo();
                this.classCount++;
                result = type;
            }
            finally
            {
                this.rwLock.DowngradeFromWriterLock(ref lockCookie);
            }
            return result;
        }

        // Token: 0x06000324 RID: 804 RVA: 0x0000B9DC File Offset: 0x00009BDC
        private FieldInfo[] GenerateProperties(TypeBuilder tb, DynamicProperty[] properties)
        {
            FieldInfo[] array = new FieldBuilder[properties.Length];
            for (int i = 0; i < properties.Length; i++)
            {
                DynamicProperty dynamicProperty = properties[i];
                FieldBuilder fieldBuilder = tb.DefineField("_" + dynamicProperty.Name, dynamicProperty.Type, FieldAttributes.Private);
                PropertyBuilder propertyBuilder = tb.DefineProperty(dynamicProperty.Name, PropertyAttributes.HasDefault, dynamicProperty.Type, null);
                MethodBuilder methodBuilder = tb.DefineMethod("get_" + dynamicProperty.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.SpecialName, dynamicProperty.Type, Type.EmptyTypes);
                ILGenerator ilgenerator = methodBuilder.GetILGenerator();
                ilgenerator.Emit(OpCodes.Ldarg_0);
                ilgenerator.Emit(OpCodes.Ldfld, fieldBuilder);
                ilgenerator.Emit(OpCodes.Ret);
                MethodBuilder methodBuilder2 = tb.DefineMethod("set_" + dynamicProperty.Name, MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.HideBySig | MethodAttributes.SpecialName, null, new Type[]
                {
                    dynamicProperty.Type
                });
                ILGenerator ilgenerator2 = methodBuilder2.GetILGenerator();
                ilgenerator2.Emit(OpCodes.Ldarg_0);
                ilgenerator2.Emit(OpCodes.Ldarg_1);
                ilgenerator2.Emit(OpCodes.Stfld, fieldBuilder);
                ilgenerator2.Emit(OpCodes.Ret);
                propertyBuilder.SetGetMethod(methodBuilder);
                propertyBuilder.SetSetMethod(methodBuilder2);
                array[i] = fieldBuilder;
            }
            return array;
        }

        // Token: 0x06000325 RID: 805 RVA: 0x0000BB1C File Offset: 0x00009D1C
        private void GenerateEquals(TypeBuilder tb, FieldInfo[] fields)
        {
            MethodBuilder methodBuilder = tb.DefineMethod("Equals", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, typeof(bool), new Type[]
            {
                typeof(object)
            });
            ILGenerator ilgenerator = methodBuilder.GetILGenerator();
            LocalBuilder local = ilgenerator.DeclareLocal(tb);
            Label label = ilgenerator.DefineLabel();
            ilgenerator.Emit(OpCodes.Ldarg_1);
            ilgenerator.Emit(OpCodes.Isinst, tb);
            ilgenerator.Emit(OpCodes.Stloc, local);
            ilgenerator.Emit(OpCodes.Ldloc, local);
            ilgenerator.Emit(OpCodes.Brtrue_S, label);
            ilgenerator.Emit(OpCodes.Ldc_I4_0);
            ilgenerator.Emit(OpCodes.Ret);
            ilgenerator.MarkLabel(label);
            foreach (FieldInfo fieldInfo in fields)
            {
                Type fieldType = fieldInfo.FieldType;
                Type type = typeof(EqualityComparer<>).MakeGenericType(new Type[]
                {
                    fieldType
                });
                label = ilgenerator.DefineLabel();
                ilgenerator.EmitCall(OpCodes.Call, type.GetMethod("get_Default"), null);
                ilgenerator.Emit(OpCodes.Ldarg_0);
                ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
                ilgenerator.Emit(OpCodes.Ldloc, local);
                ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
                ilgenerator.EmitCall(OpCodes.Callvirt, type.GetMethod("Equals", new Type[]
                {
                    fieldType,
                    fieldType
                }), null);
                ilgenerator.Emit(OpCodes.Brtrue_S, label);
                ilgenerator.Emit(OpCodes.Ldc_I4_0);
                ilgenerator.Emit(OpCodes.Ret);
                ilgenerator.MarkLabel(label);
            }
            ilgenerator.Emit(OpCodes.Ldc_I4_1);
            ilgenerator.Emit(OpCodes.Ret);
        }

        // Token: 0x06000326 RID: 806 RVA: 0x0000BCD4 File Offset: 0x00009ED4
        private void GenerateGetHashCode(TypeBuilder tb, FieldInfo[] fields)
        {
            MethodBuilder methodBuilder = tb.DefineMethod("GetHashCode", MethodAttributes.FamANDAssem | MethodAttributes.Family | MethodAttributes.Virtual | MethodAttributes.HideBySig, typeof(int), Type.EmptyTypes);
            ILGenerator ilgenerator = methodBuilder.GetILGenerator();
            ilgenerator.Emit(OpCodes.Ldc_I4_0);
            foreach (FieldInfo fieldInfo in fields)
            {
                Type fieldType = fieldInfo.FieldType;
                Type type = typeof(EqualityComparer<>).MakeGenericType(new Type[]
                {
                    fieldType
                });
                ilgenerator.EmitCall(OpCodes.Call, type.GetMethod("get_Default"), null);
                ilgenerator.Emit(OpCodes.Ldarg_0);
                ilgenerator.Emit(OpCodes.Ldfld, fieldInfo);
                ilgenerator.EmitCall(OpCodes.Callvirt, type.GetMethod("GetHashCode", new Type[]
                {
                    fieldType
                }), null);
                ilgenerator.Emit(OpCodes.Xor);
            }
            ilgenerator.Emit(OpCodes.Ret);
        }

        // Token: 0x0400010D RID: 269
        public static readonly ClassFactory Instance = new ClassFactory();

        // Token: 0x0400010E RID: 270
        private ModuleBuilder module;

        // Token: 0x0400010F RID: 271
        private Dictionary<Signature, Type> classes;

        // Token: 0x04000110 RID: 272
        private int classCount;

        // Token: 0x04000111 RID: 273
        private ReaderWriterLock rwLock;
    }
}
