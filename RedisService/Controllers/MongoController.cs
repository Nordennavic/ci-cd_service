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
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using ServiceStack.Redis;

namespace RedisService.Controllers
{
    public class MongoController : Controller
    {
        private MongoClient mongoClient;
        private readonly IOptions<MongoConf> mongoConfig;
        public MongoController(IOptions<MongoConf> MongoConfig)
        {
            this.mongoConfig = MongoConfig;
            mongoClient = new MongoClient(mongoConfig.Value.MongoConnectionString);
        }

        [HttpGet]
        public ActionResult Index()
        {
            return StatusCode(200, "Vso ok");
        }


        [HttpGet]
        public IActionResult AddByKey([FromQuery(Name = "key")] string key, [FromQuery(Name = "value")] string value, [FromQuery(Name = "expiresAt")] string expiresAt)
        {
            var mongoDatabase = mongoClient.GetDatabase("my_database");
            var col = mongoDatabase.GetCollection<BsonDocument>("accounts");
            var a = new Account()
            {
                Nickname = "Nordennavic", Library = new List<string>() {"Ijiraraide, Nagatoro-san!", "Goblin Slayer"}, Birthday = DateTime.Today,
                Email = "liptonb@ya.ru"
            };
            col.InsertOne(a.ToBsonDocument());
            return new OkResult();
        }

        //[HttpGet]
        //public async Task<string> GetByKey([FromQuery(Name = "key")] string key)
        //{
        //    using var client = redisManager.GetClient();
        //    var value = client.GetValue(key);
        //    return value;
        //}

        //[HttpGet]
        //public async Task<string> DeleteByKey([FromQuery(Name = "key")] string key)
        //{
        //    using var client = redisManager.GetClient();
        //    if (client.Remove(key))
        //        return $"Value with key {key} successfully removed!";
        //    return $"Value with key {key} Not Found.";
        //}

        //[HttpGet]
        //public async Task<string> SearchByKey([FromQuery(Name = "pattern")] string pattern)
        //{
        //    using var client = redisManager.GetClient();
        //    IList<string> keys = client.SearchKeys($"*{pattern}*");
        //    var result = $"By pattern \"{pattern}\" {keys.Count} matches found: \n";
        //    foreach (var key in keys) result += key + "\n";
        //    return result;
        //}

    }
}
