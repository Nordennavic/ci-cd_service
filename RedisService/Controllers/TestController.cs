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

        [HttpGet]
        public ActionResult Test()
        {
            return StatusCode(200, "Vso tochno ok");
        }

        [HttpPost]
        [Route("/teoInbound/universal")]
        public IActionResult AddByKey()
        {
            return StatusCode(503, "Service currently unavailable");
        }
    }
}
