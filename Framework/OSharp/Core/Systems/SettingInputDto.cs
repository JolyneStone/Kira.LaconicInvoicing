// -----------------------------------------------------------------------
//  <copyright file="SettingInputDto.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-24 17:21</last-date>
// -----------------------------------------------------------------------

namespace OSharp.Core.Systems
{
    /// <summary>
    /// ������Ϣ����DTO
    /// </summary>
    public class SettingInputDto
    {
        /// <summary>
        /// ��ȡ������ ��������ȫ��
        /// </summary>
        public string SettingTypeName { get; set; }

        /// <summary>
        /// ��ȡ������ ����ģ��JSON
        /// </summary>
        public string SettingJson { get; set; }
    }
}