// -----------------------------------------------------------------------
//  <copyright file="TypeMetadataHandler.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-06 13:44</last-date>
// -----------------------------------------------------------------------

using System;
using System.Linq;

using OSharp.Entity;
using OSharp.Reflection;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// ����Ԫ���ݴ�����
    /// </summary>
    public class TypeMetadataHandler : ITypeMetadataHandler
    {
        private readonly IEntityTypeFinder _entityTypeFinder;
        private readonly IInputDtoTypeFinder _inputDtoTypeFinder;
        private readonly IOutputDtoTypeFinder _outputDtoTypeFinder;

        /// <summary>
        /// ��ʼ��һ��<see cref="TypeMetadataHandler"/>���͵���ʵ��
        /// </summary>
        public TypeMetadataHandler(IEntityTypeFinder entityTypeFinder,
            IInputDtoTypeFinder inputDtoTypeFinder,
            IOutputDtoTypeFinder outputDtoTypeFinder)
        {
            _entityTypeFinder = entityTypeFinder;
            _inputDtoTypeFinder = inputDtoTypeFinder;
            _outputDtoTypeFinder = outputDtoTypeFinder;
        }

        /// <summary>
        /// ��ȡʵ�����Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        public TypeMetadata[] GetEntityTypeMetadatas()
        {
            Type[] entityTypes = _entityTypeFinder.Find(m => !m.HasAttribute<IgnoreGenTypeAttribute>());
            return entityTypes.Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// ��ȡ����DTO���͵�Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        public TypeMetadata[] GetInputDtoMetadatas()
        {
            Type[] inputDtoTypes = _inputDtoTypeFinder.Find(m => !m.HasAttribute<IgnoreGenTypeAttribute>());
            return inputDtoTypes.Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// ��ȡ���DTO���͵�Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        public TypeMetadata[] GetOutputDtoMetadata()
        {
            Type[] outDtoTypes = _outputDtoTypeFinder.Find(m => !m.HasAttribute<IgnoreGenTypeAttribute>());
            return outDtoTypes.Select(m => new TypeMetadata(m)).ToArray();
        }

        /// <summary>
        /// ��ȡָ�����͵�Ԫ����
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>Ԫ����</returns>
        public TypeMetadata GetTypeMetadata(Type type)
        {
            if (type == null)
            {
                return null;
            }
            return new TypeMetadata(type);
        }
    }
}