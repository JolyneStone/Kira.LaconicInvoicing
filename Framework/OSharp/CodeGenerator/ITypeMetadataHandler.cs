// -----------------------------------------------------------------------
//  <copyright file="ITypeMetadataHandler.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-06 13:42</last-date>
// -----------------------------------------------------------------------

using System;


namespace OSharp.CodeGenerator
{
    /// <summary>
    /// ����Ԫ���ݴ�����
    /// </summary>
    public interface ITypeMetadataHandler
    {
        /// <summary>
        /// ��ȡʵ�����Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        TypeMetadata[] GetEntityTypeMetadatas();

        /// <summary>
        /// ��ȡ����DTO���͵�Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        TypeMetadata[] GetInputDtoMetadatas();

        /// <summary>
        /// ��ȡ���DTO���͵�Ԫ����
        /// </summary>
        /// <returns>Ԫ���ݼ���</returns>
        TypeMetadata[] GetOutputDtoMetadata();

        /// <summary>
        /// ��ȡָ�����͵�Ԫ����
        /// </summary>
        /// <param name="type">����</param>
        /// <returns>Ԫ����</returns>
        TypeMetadata GetTypeMetadata(Type type);
    }
}