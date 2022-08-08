using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wbf.VOL.Core.CacheManager.IService;

namespace Wbf.VOL.Core.CacheManager.Service
{
    public class RedisCacheService : ICacheService
    {
        public RedisCacheService()
        {
            //var csredis = new CSRedisClient(AppSetting.RedisConnectionString);
            //RedisHelper.Initialization(csredis);
        }

        public bool AddObject(string key, object value, int expireSeconds = -1, bool isSliding = false)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            
        }

        public bool Exists(string key)
        {
            throw new NotImplementedException();
        }

        public T Get<T>(string key) where T : class
        {
            throw new NotImplementedException();
        }

        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }
    }
}
