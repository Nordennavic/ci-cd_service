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
    public class TestController : Controller
    {
        public TestController()
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return StatusCode(200, "Vso ok");
        }


        [HttpPost]
        [Route("/teoInbound/universal")]
        public IActionResult AddByKey()
        {
            return StatusCode(503, "Service currently unavailable");
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
