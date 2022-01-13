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
using ServiceStack.Redis;

namespace RedisService.Controllers
{
    public class RedisController : Controller
    {
        private RedisManagerPool redisManager;
        private readonly IOptions<RedisConf> redisConfig;
        public RedisController(IOptions<RedisConf> RedisConfig)
        {
            this.redisConfig = RedisConfig;
            redisManager = new RedisManagerPool(redisConfig.Value.RedisConnectionString);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return StatusCode(200, "Vso ok");
        }


        [HttpGet]
        public async Task<ActionResult> AddByKey([FromQuery(Name = "key")] string key, [FromQuery(Name = "value")] string value, [FromQuery(Name = "expiresAt")] string expiresAt)
        {
            var expiresTime = TimeSpan.FromMinutes(5);
            if (expiresAt != null)
                expiresTime = TimeSpan.FromMinutes(int.Parse(expiresAt));

            using var client = redisManager.GetClient();
            client.Set(key, value, expiresTime);
            return StatusCode(200, $"Value added by key {key}");
        }

        [HttpGet]
        public async Task<string> GetByKey([FromQuery(Name = "key")] string key)
        {
            using var client = redisManager.GetClient();
            var value = client.GetValue(key);
            return value;
        }

        [HttpGet]
        public async Task<string> DeleteByKey([FromQuery(Name = "key")] string key)
        {
            using var client = redisManager.GetClient();
            if (client.Remove(key))
                return $"Value with key {key} successfully removed!";
            return $"Value with key {key} Not Found.";
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
