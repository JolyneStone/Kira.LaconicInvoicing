using System.Collections.Generic;

using OSharp.CodeGeneration.Schema;

using Shouldly;

using Xunit;

namespace OSharp.CodeGeneration.Tests
{
    public class RazorCodeGeneratorTests
    {
        private readonly ModuleMetadata _module;
        private readonly EntityMetadata _entity;
        private readonly ICodeGenerator _generator;

        public RazorCodeGeneratorTests()
        {
            _generator = new RazorCodeGenerator();

            ProjectMetadata project = new ProjectMetadata()
            {
                Name = "XXXϵͳ",
                Company = "�������",
                Copyright = "Copyright (c) 2014-2019 OSharp. All rights reserved.",
                Creator = "������",
                SiteUrl = "https://www.66soft.net",
                NamespacePrefix = "Liuliu.Site"
            };
            _module = new ModuleMetadata()
            {
                Name = "Infos",
                Display = "��Ϣ",
                Project = project
            };
            _entity = new EntityMetadata()
            {
                Name = "Article",
                Display = "����",
                Module = _module,
                IsDataAuth = true,
                PrimaryKeyTypeFullName = typeof(int).FullName,
                Properties = new List<PropertyMetadata>()
                {
                    new PropertyMetadata() { Name = "Title", Display = "����", TypeName = typeof(string).FullName },
                    new PropertyMetadata() { Name = "Content", Display = "����", TypeName = typeof(string).FullName },
                    new PropertyMetadata() { Name = "IsHot", Display = "�Ƿ��ȵ�", TypeName = typeof(bool).FullName },
                    new PropertyMetadata() { Name = "IsLocked", Display = "�Ƿ�����", TypeName = typeof(bool).FullName },
                    new PropertyMetadata() { Name = "IsDeleted", Display = "�Ƿ�ɾ��", TypeName = typeof(bool).FullName, IsInputDto = false, IsOutputDto = false }
                }
            };
        }

        [Fact]
        public void GenerateProjectCodeTest()
        {
            CodeFile[] codes = _generator.GenerateProjectCode(_module.Project);
            codes.Length.ShouldBeGreaterThan(0);
        }

        [Fact]
        public void GenerateEntityCodeTest()
        {
            CodeFile code = _generator.GenerateEntityCode(_entity);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/Entities/Article.generated.cs");
        }

        [Fact]
        public void GenerateEntityConfigurationTest()
        {
            CodeFile code = _generator.GenerateEntityConfiguration(_entity);
            AssertCodeFile(code, "Liuliu.Site.EntityConfiguration/Infos/ArticleConfiguration.generated.cs");
        }

        [Fact]
        public void GenerateInputDtoCodeTest()
        {
            CodeFile code = _generator.GenerateInputDtoCode(_entity);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/Dtos/ArticleInputDto.generated.cs");
        }

        [Fact]
        public void GenerateOutputDtoCodeTest()
        {
            CodeFile code = _generator.GenerateOutputDtoCode(_entity);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/Dtos/ArticleOutputDto.generated.cs");
        }

        [Fact]
        public void GenerateServiceContractTest()
        {
            CodeFile code = _generator.GenerateServiceContract(_module);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/IInfosContract.generated.cs");
        }

        [Fact]
        public void GenerateServiceMainImplTest()
        {
            CodeFile code = _generator.GenerateServiceMainImpl(_module);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/InfosService.generated.cs");
        }

        [Fact]
        public void GenerateServiceEntityImplTest()
        {
            CodeFile code = _generator.GenerateServiceEntityImpl(_entity);
            AssertCodeFile(code, "Liuliu.Site.Core/Infos/InfosService.Article.generated.cs");
        }

        [Fact]
        public void GenerateAdminControllerTest()
        {
            CodeFile code = _generator.GenerateAdminController(_entity);
            AssertCodeFile(code, "Liuliu.Site.Web/Areas/Admin/Controllers/Infos/ArticleController.generated.cs");
        }

        private static void AssertCodeFile(CodeFile code, string checkFileName)
        {
            code.FileName.ShouldBe(checkFileName);
            code.SourceCode.ShouldContain("namespace");
            code.SourceCode.ShouldContain("using");
            code.SourceCode.ShouldContain("{");
            code.SourceCode.ShouldContain("}");
        }

    }
}
