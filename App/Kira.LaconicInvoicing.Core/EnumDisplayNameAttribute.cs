using System;
using System.ComponentModel;

namespace Kira.LaconicInvoicing
{
    //
    // Summary:
    //     指定枚举或枚举值的显示名称。
    [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field, AllowMultiple = false)]
    public class EnumDisplayNameAttribute : DisplayNameAttribute
    {
        //
        // Summary:
        //     初始化 YuLinTu.EnumDisplayNameAttribute 类的新实例。
        public EnumDisplayNameAttribute() { }
        //
        // Summary:
        //     根据显示名称初始化 YuLinTu.EnumDisplayNameAttribute 类的新实例。
        //
        // Parameters:
        //   displayName:
        //     显示名称。
        public EnumDisplayNameAttribute(string displayName):base(displayName) { }
    }
}
