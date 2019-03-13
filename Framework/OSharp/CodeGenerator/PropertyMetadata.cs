// -----------------------------------------------------------------------
//  <copyright file="PropertyMetadata.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-06 12:31</last-date>
// -----------------------------------------------------------------------

using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

using OSharp.Reflection;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// ����Ԫ����
    /// </summary>
    public class PropertyMetadata
    {
        /// <summary>
        /// ��ʼ��һ��<see cref="PropertyMetadata"/>���͵���ʵ��
        /// </summary>
        public PropertyMetadata()
        { }

        /// <summary>
        /// ��ʼ��һ��<see cref="PropertyMetadata"/>���͵���ʵ��
        /// </summary>
        public PropertyMetadata(PropertyInfo property)
        {
            if (property == null)
            {
                return;
            }

            Name = property.Name;
            TypeName = property.PropertyType.FullName;
            Display = property.GetDescription();
            RequiredAttribute required = property.GetAttribute<RequiredAttribute>();
            if (required != null)
            {
                IsRequired = !required.AllowEmptyStrings;
            }
            StringLengthAttribute stringLength = property.GetAttribute<StringLengthAttribute>();
            if (stringLength != null)
            {
                MaxLength = stringLength.MaximumLength;
                MinLength = stringLength.MinimumLength;
            }
            else
            {
                MaxLength = property.GetAttribute<MaxLengthAttribute>()?.Length;
                MinLength = property.GetAttribute<MinLengthAttribute>()?.Length;
            }
            RangeAttribute range = property.GetAttribute<RangeAttribute>();
            if (range != null)
            {
                Range = new[] { range.Minimum, range.Maximum };
                Max = range.Maximum;
                Min = range.Minimum;
            }
            IsNullable = property.PropertyType.IsNullableType();
            if (IsNullable)
            {
                TypeName = property.PropertyType.GetUnNullableType().FullName;
            }
            //ö�����ͣ���Ϊ��ֵ���ͷ���
            if (property.PropertyType.IsEnum)
            {
                Type enumType = property.PropertyType;
                Array values = enumType.GetEnumValues();
                Enum[] enumItems = values.Cast<Enum>().ToArray();
                if (enumItems.Length > 0)
                {
                    EnumMetadatas = enumItems.Select(m => new EnumMetadata(m)).ToArray();
                }
            }
        }
        
        /// <summary>
        /// ��ȡ������ ��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��ȡ������ ������������
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        /// ��ȡ������ ��ʾ����
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ����
        /// </summary>
        public bool? IsRequired { get; set; }

        /// <summary>
        /// ��ȡ������ ��󳤶�
        /// </summary>
        public int? MaxLength { get; set; }

        /// <summary>
        /// ��ȡ������ ��С����
        /// </summary>
        public int? MinLength { get; set; }

        /// <summary>
        /// ��ȡ������ ȡֵ��Χ
        /// </summary>
        public object[] Range { get; set; }

        /// <summary>
        /// ��ȡ������ ���ֵ
        /// </summary>
        public object Max { get; set; }

        /// <summary>
        /// ��ȡ������ ��Сֵ
        /// </summary>
        public object Min { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ�ֵ���Ϳɿ�
        /// </summary>
        public bool IsNullable { get; set; }

        /// <summary>
        /// ��ȡ������ ö��Ԫ����
        /// </summary>
        public EnumMetadata[] EnumMetadatas { get; set; }

        /// <summary>
        /// �Ƿ�����֤���� 
        /// </summary>
        public bool HasValidateAttribute()
        {
            return IsRequired.HasValue || MaxLength.HasValue || MinLength.HasValue || Range != null || Max != null || Min != null;
        }
    }
}