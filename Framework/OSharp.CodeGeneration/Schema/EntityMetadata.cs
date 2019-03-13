// -----------------------------------------------------------------------
//  <copyright file="EntityMetadata.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-06 12:25</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;

using OSharp.Collections;
using OSharp.Exceptions;


namespace OSharp.CodeGeneration.Schema
{
    /// <summary>
    /// ʵ��Ԫ����
    /// </summary>
    public class EntityMetadata
    {
        private ModuleMetadata _module;

        /// <summary>
        /// ��ȡ������ ��������
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��ȡ������ ������ʾ����
        /// </summary>
        public string Display { get; set; }

        /// <summary>
        /// ��ȡ������ ��������ȫ��
        /// </summary>
        public string PrimaryKeyTypeFullName { get; set; }

        /// <summary>
        /// ��ȡ������ �Ƿ�����Ȩ�޿���
        /// </summary>
        public bool IsDataAuth { get; set; }

        /// <summary>
        /// ��ȡ������ ����ģ����Ϣ
        /// </summary>
        public ModuleMetadata Module
        {
            get => _module;
            set
            {
                _module = value;
                value.Entities.AddIfNotExist(this);
            }
        }

        /// <summary>
        /// ��ȡ������ ʵ������Ԫ���ݼ���
        /// </summary>
        public ICollection<PropertyMetadata> Properties { get; set; } = new List<PropertyMetadata>();
    }
}