﻿// -----------------------------------------------------------------------
//  <copyright file="AbstractBuilder.cs" company="OSharp开源团队">
//      Copyright (c) 2014 OSharp. All rights reserved.
//  </copyright>
//  <last-editor>郭明锋</last-editor>
//  <last-date>2014:07:05 2:53</last-date>
// -----------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;

using OSharp.Entity;
using OSharp.Core.EntityInfos;

using Xunit;

using OSharp.UnitTest.Infrastructure;


namespace OSharp.Reflection.Tests
{

    public class TypeExtensionsTests
    {
        [Fact]
        public void IsDeriveFromTest()
        {
            Assert.True(typeof(EntityInfo).IsDeriveClassFrom<IEntityInfo>());
            Assert.True(typeof(EntityInfo).IsDeriveClassFrom(typeof(IEntity<>)));
        }

        [Fact()]
        public void IsNullableTypeTest()
        {
            // ReSharper disable ConvertNullableToShortForm
            Assert.True(typeof(int?).IsNullableType());
            Assert.True(typeof(Nullable<int>).IsNullableType());

            Assert.False(typeof(int).IsNullableType());
        }

        [Fact()]
        public void IsEnumerableTest()
        {
            Assert.True(typeof(string[]).IsEnumerable());
            Assert.True(typeof(ICollection<string>).IsEnumerable());
            Assert.True(typeof(IEnumerable<string>).IsEnumerable());
            Assert.True(typeof(IList<string>).IsEnumerable());
            Assert.True(typeof(Hashtable).IsEnumerable());
            Assert.True(typeof(HashSet<string>).IsEnumerable());

            Assert.False(typeof(int).IsEnumerable());
            Assert.False(typeof(string).IsEnumerable());
        }

        [Fact()]
        public void GetNonNummableType()
        {
            Assert.Equal(typeof(int), typeof(int?).GetNonNummableType());
            Assert.Equal(typeof(int), typeof(Nullable<int>).GetNonNummableType());

            Assert.Equal(typeof(int), typeof(int).GetNonNummableType());
        }

        [Fact()]
        public void GetUnNullableTypeTest()
        {
            Assert.Equal(typeof(int), typeof(int?).GetUnNullableType());
            Assert.Equal(typeof(int), typeof(Nullable<int>).GetUnNullableType());

            Assert.Equal(typeof(int), typeof(int).GetUnNullableType());
        }

        [Fact()]
        public void ToDescriptionTest()
        {
            Type type = typeof(TestEntity);
            Assert.Equal("测试实体", type.GetDescription());
            PropertyInfo property = type.GetProperty("Id");
            Assert.Equal("编号", property.GetDescription());

            type = GetType();
            Assert.Equal("OSharp.Reflection.Tests.TypeExtensionsTests", type.GetDescription());
        }

        [Fact()]
        public void HasAttributeTest()
        {
            Type type = GetType();
            MethodInfo method = type.GetMethod("HasAttributeTest");
            Assert.True(method.HasAttribute<FactAttribute>());
        }

        [Fact()]
        public void GetAttributeTest()
        {
            Type type = typeof(TestEntity);
            Assert.Equal("测试实体", type.GetAttribute<DescriptionAttribute>().Description);
            PropertyInfo property = type.GetProperty("Id");
            Assert.Equal("编号", property.GetAttribute<DescriptionAttribute>().Description);
            MethodInfo method = GetType().GetMethod("GetAttributeTest");
            Assert.False(method.GetAttribute<FactAttribute>() == null);
        }

        [Fact()]
        public void GetAttributesTest()
        {
            Type type = GetType();
            Assert.Empty(type.GetAttributes<DescriptionAttribute>());
        }

        [Fact()]
        public void IsGenericAssignableFromTest()
        {
            Assert.True(typeof(IEnumerable<>).IsGenericAssignableFrom(typeof(List<>)));
            Assert.True(typeof(List<>).IsGenericAssignableFrom(typeof(List<string>)));

            Assert.Throws<ArgumentException>(() =>
                (typeof(string)).IsGenericAssignableFrom(typeof(int)));
        }

        [Fact()]
        public void IsBaseOnTest()
        {
            Assert.True(typeof(List<>).IsBaseOn(typeof(List<>)));
            Assert.True(typeof(List<>).IsBaseOn(typeof(IList<>)));
            Assert.True(typeof(List<string>).IsBaseOn(typeof(List<string>)));
            Assert.True(typeof(List<string>).IsBaseOn(typeof(IList<string>)));

            Assert.True(typeof(string).IsBaseOn<IEnumerable>());
        }
    }
}