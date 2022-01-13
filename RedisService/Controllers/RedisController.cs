using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using RedisService.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Serializers;
using ServiceStack.Redis;

namespace RedisService.Controllers
{
    public class RedisController : Controller
    {
        public IRedisClientsManager redisManager;
        private readonly IOptions<RedisConf> redisConfig;
        public RedisController(IOptions<RedisConf> RedisConfig)
        {
            this.redisConfig = RedisConfig;
            redisManager = new RedisManagerPool(redisConfig?.Value?.RedisConnectionString);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return StatusCode(200, "Vso ok");
        }


        [HttpGet]
        public async Task<dynamic> AddByKey([FromQuery(Name = "key")] string key, [FromQuery(Name = "value")] string value, [FromQuery(Name = "expiresAt")] string expiresAt)
        {
            if (key != null && value != null)
            {
                var expiresTime = TimeSpan.FromMinutes(5);
                if (expiresAt != null)
                    expiresTime = TimeSpan.FromMinutes(int.Parse(expiresAt));

                using var client = redisManager.GetClient();
                client.Set(key, value, expiresTime);
                return StatusCode(200, $"Value added by key {key}");
            }

            return StatusCode(400, "Value or key not set");
        }

        [HttpGet]
        public async Task<string> GetByKey([FromQuery(Name = "key")] string key)
        {
            if (key != null)
            {
                using var client = redisManager.GetClient();
                var value = client.GetValue(key);
                if (value != null)
                    return value;
                return $"Value by key \"{key}\" not found";
            }

            return "Key not set";
        }

        [HttpGet]
        public async Task<string> DeleteByKey([FromQuery(Name = "key")] string key)
        {
            if (key != null)
            {
                using var client = redisManager.GetClient();
                if (client.Remove(key))
                    return $"Value by key {key} successfully removed";
                return $"Value by key {key} not found";
            }

            return "Key not set";
        }

        [HttpGet]
        public async Task<string> SearchByKey([FromQuery(Name = "pattern")] string pattern)
        {
            using var client = redisManager.GetClient();
            IList<string> keys = client.SearchKeys($"*{pattern}*");
            var result = $"By pattern \"{pattern}\" {keys.Count} matches found: \n";
            foreach (var key in keys) result += key + "\n";
            return result;
        }

    }
}
