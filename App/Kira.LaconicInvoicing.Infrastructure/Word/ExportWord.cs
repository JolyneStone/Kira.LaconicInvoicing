using Aspose.Words;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Kira.LaconicInvoicing.Infrastructure.Word
{
    public class ExportWord
    {
        private readonly Document _document;
        public Action OnExport { get; set; }

        public ExportWord(string templateFile)
        {
            if (string.IsNullOrWhiteSpace(templateFile))
            {
                throw new ArgumentNullException(nameof(templateFile));
            }

            if (!File.Exists(templateFile))
            {
                throw new FileNotFoundException("模板文件不存在", nameof(templateFile));
            }

            try
            {
                _document = new Document(templateFile);
            }
            catch (Exception ex)
            {
                throw new FileLoadException("模板文件加载失败。", ex);
            }
        }

        public void Export(string targetFileName)
        {
            try
            {
                OnExport?.Invoke();
                _document.Save(targetFileName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("导出 Word 文档失败。", ex);
            }
        }

        public void SetBookMarkText(string markName, string text)
        {
            if (!string.IsNullOrEmpty(markName))
            {
                text = string.IsNullOrEmpty(text) ? " " : text;
                var mark = _document.Range.Bookmarks[markName];
                if (mark != null)
                    mark.Text = text;
            }
        }
    }
}
