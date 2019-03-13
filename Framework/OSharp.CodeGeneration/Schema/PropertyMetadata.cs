// -----------------------------------------------------------------------
//  <copyright file="PropertyMetadata.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-06 12:31</last-date>
// -----------------------------------------------------------------------


using OSharp.Collections;


namespace OSharp.CodeGeneration.Schema
{
    /// <summary>
    /// ����Ԫ����
    /// </summary>
    public class PropertyMetadata
    {
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
        /// ��ȡ������ �Ƿ�������
        /// </summary>
        public bool IsVirtual { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ����
        /// </summary>
        public bool IsForeignKey { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ����������Dto
        /// </summary>
        public bool IsInputDto { get; set; } = true;

        /// <summary>
        /// ��ȡ������ �Ƿ���������Dto
        /// </summary>
        public bool IsOutputDto { get; set; } = true;

        /// <summary>
        /// ��ȡ������ ö��Ԫ����
        /// </summary>
        public EnumMetadata[] EnumMetadatas { get; set; }

        private EntityMetadata _entity;
        /// <summary>
        /// ��ȡ������ ����ʵ����Ϣ
        /// </summary>
        public EntityMetadata Entity
        {
            get => _entity;
            set
            {
                _entity = value;
                value.Properties.AddIfNotExist(this);
            }
        }

        /// <summary>
        /// �Ƿ�����֤���� 
        /// </summary>
        public bool HasValidateAttribute()
        {
            return IsRequired.HasValue || MaxLength.HasValue || MinLength.HasValue || Range != null || Max != null || Min != null;
        }

        /// <summary>
        /// ��ȡ���Ա�ʾ���͵ļ����ͣ��� System.String ���� string
        /// </summary>
        public string ToSingleTypeName()
        {
            return TypeHelper.ToSingleTypeName(TypeName, IsNullable);
        }

        /// <summary>
        /// ��ȡ���Ա�ʾ���͵�JS�����ַ���
        /// </summary>
        public string ToJsTypeName()
        {
            PropertyMetadata prop = this;
            string name = "object";
            switch (prop.TypeName)
            {
                case "System.Byte":
                case "System.Int32":
                case "System.Int64":
                case "System.Decimal":
                case "System.Single":
                case "System.Double":
                    name = "number";
                    break;
                case "System.String":
                case "System.Guid":
                    name = "string";
                    break;
                case "System.Boolean":
                    name = "boolean";
                    break;
                case "System.DateTime":
                    name = "date";
                    break;
            }
            if (prop.EnumMetadatas != null)
            {
                name = "number";
            }
            return name;
        }
    }
}