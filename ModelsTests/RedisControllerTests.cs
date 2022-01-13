using RedisService.Controllers;
using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Options;
using Moq;
using RedisService.Models;
using ServiceStack.Redis;
using StackExchange.Redis;
using Xunit;
using ServiceStack.Caching;
using System.Collections.Generic;

namespace ModelsTests
{
    public class RedisControllerTests
    {
        [Fact]
        public async void AddByKeyOkTest()
        {
            var options = prepareOptions();
            var redisController = prepareRedisController(options);
            ObjectResult result = await redisController.AddByKey("key", "value", null);
            Assert.True(200 == result?.StatusCode.Value);
        }

        [Fact]
        public async void AddByKeyFalseTest()
        {
            var options = prepareOptions();
            var redisController = prepareRedisController(options);
            ObjectResult result = await redisController.AddByKey(null, null, null);
            Assert.True(400 == result?.StatusCode.Value);
        }

        private IOptions<RedisConf> prepareOptions()
        {
            var conf = new RedisConf() {RedisConnectionString = ""};
            var options = Options.Create(conf);
            return options;
        }

        private RedisController prepareRedisController(IOptions<RedisConf> conf)
        {
            var controller = new RedisController(conf)
            {
                ControllerContext = new ControllerContext() {HttpContext = new DefaultHttpContext()}
            };
            var mockRedisManagerPool = new Mock<IRedisClientsManager>();
            var mockIRedisClient = new Mock<IRedisClient>();
            mockIRedisClient.Setup(foo => foo.Set("", "")).Returns(true);
            mockRedisManagerPool.Setup(foo => foo.GetClient()).Returns(mockIRedisClient.Object);
            controller.redisManager = mockRedisManagerPool.Object;
            return controller;
        }

    }
}
