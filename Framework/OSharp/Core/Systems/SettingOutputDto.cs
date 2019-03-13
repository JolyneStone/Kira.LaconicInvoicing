// -----------------------------------------------------------------------
//  <copyright file="SettingOutputDto.cs" company="OSharp��Դ�Ŷ�">
//      Copyright (c) 2014-2018 OSharp. All rights reserved.
//  </copyright>
//  <site>http://www.osharp.org</site>
//  <last-editor>������</last-editor>
//  <last-date>2018-08-24 17:26</last-date>
// -----------------------------------------------------------------------

using OSharp.Reflection;


namespace OSharp.Core.Systems
{
    /// <summary>
    /// �������DTO
    /// </summary>
    public class SettingOutputDto
    {
        /// <summary>
        /// ��ʼ��һ��<see cref="SettingOutputDto"/>���͵���ʵ��
        /// </summary>
        public SettingOutputDto(ISetting setting)
        {
            Setting = setting;
            SettingTypeName = setting.GetType().GetFullNameWithModule();
        }

        /// <summary>
        /// ��ȡ ��������ȫ��
        /// </summary>
        public string SettingTypeName { get; }

        /// <summary>
        /// ��ȡ ������Ϣ
        /// </summary>
        public ISetting Setting { get; }
    }
}