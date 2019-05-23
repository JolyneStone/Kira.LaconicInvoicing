using Kira.LaconicInvoicing.Infrastructure.Options;
using Kira.LaconicInvoicing.Print.Dtos;
using Kira.LaconicInvoicing.Print.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using OSharp.Entity;
using OSharp.Mapping;
using System;
using System.Text;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Options;

namespace Kira.LaconicInvoicing.Service.Print
{
    public class PrintService : IPrintContract
    {
        private readonly IRepository<PrintTemplate, Guid> _printRepo;
        private readonly ILogger<PrintService> _logger;
        private readonly string _templatesPath;

        public PrintService(IRepository<PrintTemplate, Guid> printRepo,
            ILogger<PrintService> logger)
        {
            _printRepo = printRepo;
            _logger = logger;
            _templatesPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "templates");
        }

        public async Task<string> TemplateSaveAsync(IFormFile file)
        {
            var directory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "temp");
            var directoryInfo = new DirectoryInfo(directory);
            if (!directoryInfo.Exists)
                directoryInfo.Create();
            //var fileExtend = file.FileName.Substring(file.FileName.LastIndexOf('.'));
            //var fileName = Guid.NewGuid().ToString() + fileExtend;
            var fileName = Guid.NewGuid().ToString() + ".txt";
            var fileFullName = $"{directory}{Path.DirectorySeparatorChar}{fileName}";
            using (var stream = new FileStream(fileFullName, FileMode.OpenOrCreate, FileAccess.Write))
            {
                await file.CopyToAsync(stream);
            }

            return fileName;
        }

        public async Task AddAsync(PrintTemplateInputDto dto, IFormFile file = null)
        {
            var fileName = Guid.NewGuid().ToString() + ".txt";
            var destPath = $"{_templatesPath}{Path.DirectorySeparatorChar}{fileName}";
            if (file == null)
            {
                using (var writer = System.IO.File.CreateText(destPath))
                {
                    var scripts = Regex.Split(dto.Script,"Kira.LaconicInvoicingScript", RegexOptions.IgnoreCase);
                    foreach(var script in scripts)
                    {
                        await writer.WriteLineAsync(script);
                    }
                }
            }
            else
            {
                //var sourcePath = $"{directory}{Path.DirectorySeparatorChar}temp{dto.Path}";
                //if (!File.Exists(sourcePath))
                //{
                //    throw new BussinessException("无法找到模板文件");
                //}

                //File.Move(sourcePath, destPath);

                using (var stream = new FileStream(destPath, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }
            }

            dto.Path = fileName;
            await _printRepo.InsertAsync(dto.MapTo<PrintTemplate>());
        }

        public async Task UpdateAsync(PrintTemplateInputDto dto, IFormFile file = null)
        {
            if(file != null)
            {
                //var destPath = $"{directory}{Path.DirectorySeparatorChar}template{dto.Path}";
                //if (!File.Exists(destPath))
                //{
                //    var sourcePath = $"{directory}{Path.DirectorySeparatorChar}temp{dto.Path}";
                //    File.Move(sourcePath, destPath);
                //}

                //File.Delete($"{directory}{Path.DirectorySeparatorChar}template{printTemplate.Path}");

                var fileName = Guid.NewGuid().ToString() + ".txt";
                using (var stream = new FileStream(_templatesPath + Path.DirectorySeparatorChar+ fileName, FileMode.OpenOrCreate, FileAccess.Write))
                {
                    await file.CopyToAsync(stream);
                }

                dto.Path = fileName;
            }
            else
            {
                var destPath = $"{_templatesPath}{Path.DirectorySeparatorChar}{dto.Path}";
                using (var writer = new StreamWriter(System.IO.File.Open(destPath, FileMode.Truncate, FileAccess.Write)))
                {
                    var scripts = Regex.Split(dto.Script, "Kira.LaconicInvoicingScript", RegexOptions.IgnoreCase);
                    for(var i = 0; i< scripts.Length; i++)
                    {
                        if (i > 0)
                        {
                            await writer.WriteLineAsync("Kira.LaconicInvoicingScript" + scripts[i]);
                        }
                        else
                        {
                            await writer.WriteLineAsync(scripts[i]);
                        }
                    }
                }
            }

            await _printRepo.UpdateAsync(dto.MapTo<PrintTemplate>());
        }

        public async Task DeleteAsync(Guid id)
        {
            var printTemplate = await _printRepo.GetAsync(id);
            if (printTemplate != null)
            {
                var fileInfo = new FileInfo($"{_templatesPath}{Path.DirectorySeparatorChar}{printTemplate.Path}");
                if (fileInfo.Exists)
                {
                    fileInfo.Delete();
                }

                await _printRepo.DeleteAsync(printTemplate);
            }
        }

        public async Task<PrintTemplateOutputDto> GetAsync(Guid id)
        {
            var printTemplate = (await _printRepo.GetAsync(id)).MapTo<PrintTemplateOutputDto>();
            printTemplate.Script = await GetPrintTemplateScriptAsync(printTemplate.Path);
            return printTemplate;
        }

        public async Task<List<PrintTemplateOutputDto>> GetAllAsync()
        {
            return await _printRepo.Query().Select(p => p.MapTo<PrintTemplateOutputDto>()).ToListAsync();
        }

        public async Task<List<PrintTemplateOutputDto>> GetAllByTypeAsync(TemplateType type)
        {
            return await _printRepo.Query().Where(p => p.Type == type).Select(p => p.MapTo<PrintTemplateOutputDto>()).ToListAsync();
        }

        public async Task<string> GetPrintTemplateScriptAsync(string path)
        {
            var fullPath = $"{_templatesPath}{Path.DirectorySeparatorChar}{path}";
            var file = new FileInfo(fullPath);
            if (!file.Exists)
                throw new BussinessException("无法找到指定打印模板文件");

            using (var reader = file.OpenText())
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}