using Kira.LaconicInvoicing.Common.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using OSharp.Entity;
using OSharp.Caching;
using OSharp.EventBuses;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Kira.LaconicInvoicing.Common.Dtos;
using Newtonsoft.Json;
using System.Linq;

using NBaseData = Kira.LaconicInvoicing.Common.Entities;

namespace Kira.LaconicInvoicing.Service.BaseData
{
    /// <summary>
    /// 基础数据操作实现
    /// </summary>
    public class BaseDataService : IBaseDataContract
    {
        private readonly IRepository<NBaseData.BaseData, Guid> _baseDataRepository;
        private readonly ILogger<BaseDataService> _logger;
        private readonly IDistributedCache _cache;
        private static object _syncObj = new object();

        public BaseDataService(IRepository<NBaseData.BaseData, Guid> baseDataRepository,
            IDistributedCache cache,
            ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<BaseDataService>();
            _cache = cache;
            _baseDataRepository = baseDataRepository;
        }

        #region NumberType

        public async Task<Dictionary<string, string>> GetListAsync(string type)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>((await GetOrRefreshAsync(type))?.Value);
        }

        public async Task<bool> UpdateListAsync(string type, BaseDataListDto dto)
        {
            type = type.ToUpperInvariant();
            var baseData = await GetByCacheAsync(ConstType.NumberType);
            if(baseData == null)
            {
                baseData = await _baseDataRepository.GetFirstAsync(b => b.Type == type);
            }
            Dictionary<string, string> baseDataDict;
            bool result = false;
            lock (_syncObj)
            {
                if (baseData == null)
                {
                    baseDataDict = new Dictionary<string, string>()
                    {
                        { dto.Name, dto.Code }
                    };

                    baseData = new NBaseData.BaseData()
                    {
                        Type = type,
                        Value = JsonConvert.SerializeObject(baseDataDict)
                    };

                    result = _baseDataRepository.Insert(baseData) > 0;
                }
                else if (!string.IsNullOrWhiteSpace(baseData.Value))
                {
                    baseDataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(baseData.Value);
                    if (baseDataDict.ContainsKey(dto.Name))
                    {
                        baseDataDict[dto.Name] = dto.Code;
                    }

                    baseData.Value = JsonConvert.SerializeObject(baseDataDict);
                    result = _baseDataRepository.Update(baseData) > 0;
                }
            }

            if (result)
            {
                await SetCacheAsync(type, baseData);
            }

            return result;
        }

        public async Task<bool> AddListAsync(string type, BaseDataListDto dto)
        {
            type = type.ToUpperInvariant();
            var baseData = await GetByCacheAsync(type);
            if (baseData == null)
            {
                baseData = await _baseDataRepository.GetFirstAsync(b => b.Type == type);
            }
            Dictionary<string, string> baseDataDict;
            bool result;
            lock (_syncObj)
            {
                if (baseData == null)
                {
                    baseDataDict = new Dictionary<string, string>()
                    {
                        { dto.Name, dto.Code }
                    };

                    baseData = new NBaseData.BaseData()
                    {
                        Type = type,
                        Value = JsonConvert.SerializeObject(baseDataDict)
                    };

                    result = _baseDataRepository.Insert(baseData) > 0;
                }
                else
                {
                    if (string.IsNullOrWhiteSpace(baseData.Value))
                    {
                        baseDataDict = new Dictionary<string, string>()
                        {
                            { dto.Name, dto.Code }
                        };
                    }
                    else
                    {
                        baseDataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(baseData.Value);
                        if (!baseDataDict.ContainsKey(dto.Name))
                        {
                            baseDataDict.Add(dto.Name, dto.Code);
                        }
                    }

                    baseData.Value = JsonConvert.SerializeObject(baseDataDict);
                    result = _baseDataRepository.Update(baseData) > 0;
                }
            }

            if (result)
            {
                await SetCacheAsync(type, baseData);
            }

            return result;
        }

        public async Task<bool> DeleteListAsync(string type, string name)
        {
            type = type.ToUpperInvariant();
            var baseData = await GetByCacheAsync(type);
            if (baseData == null)
            {
                baseData = await _baseDataRepository.GetFirstAsync(b => b.Type == type);
            }

            if (baseData == null || string.IsNullOrWhiteSpace(baseData.Value))
                return true;

            Dictionary<string, string> baseDataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(baseData.Value);
            var result = false;
            if (baseDataDict.ContainsKey(name))
            {
                baseDataDict.Remove(name);
                baseData.Value = JsonConvert.SerializeObject(baseDataDict);
                lock (_syncObj)
                {
                    result = _baseDataRepository.Update(baseData) > 0;
                }
            }

            if (result)
            {
                await SetCacheAsync(type, baseData);
            }

            return result;
        }

        public async Task<bool> UpdateValueAsync(string type, string value)
        {
            type = type.ToUpperInvariant();
            var baseData = await GetByCacheAsync(type);
            if(baseData == null)
            {
                baseData = new NBaseData.BaseData()
                {
                    Type = type,
                    Value = value
                };
            }
            else
            {
                baseData.Value = value;
            }

            var result = false;
            lock (_syncObj)
            {
                result = _baseDataRepository.Update(baseData) > 0;
            }

            if (result)
            {
                await SetCacheAsync(type, baseData);
            }

            return result;
        }

        public async Task<string> GetValueAsync(string type)
        {
            var baseData = await GetByCacheAsync(type);
            return baseData?.Value;
        }

        #endregion

        public async Task<NBaseData.BaseData> GetOrRefreshAsync(string type)
        {
            type = type.ToUpperInvariant();
            var key = $"BaseData.{type}";
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            return await _cache.GetAsync<NBaseData.BaseData>(key,
                async () =>
                {
                    return await _baseDataRepository.GetFirstAsync(b => b.Type == type);
                },
                options);
        }

        private async Task<NBaseData.BaseData> GetByCacheAsync(string type)
        {
            type = type.ToUpperInvariant();
            var key = $"BaseData.{type}";
            return await _cache.GetAsync<NBaseData.BaseData>(key);
        }

        private async Task SetCacheAsync(string type, NBaseData.BaseData baseData)
        {
            type = type.ToUpperInvariant();
            var key = $"BaseData.{type}";
            DistributedCacheEntryOptions options = new DistributedCacheEntryOptions();
            options.SetSlidingExpiration(TimeSpan.FromMinutes(30));
            await _cache.SetAsync(key, baseData, options);
        }
    }
}
