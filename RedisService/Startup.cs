using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using RedisService.Models;
using ServiceStack;
using ServiceStack.Redis;

namespace RedisService
{
    public class Startup
    {
        public Startup(IWebHostEnvironment env, IConfiguration configuration)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var heals = services.AddHealthChecks();
            var redisConf = Configuration?.GetSection("RedisSection");
            var mongoConnectionString = Configuration?.GetSection("MongoSection").GetSection("MongoConnectionString").Value;
            var redCS = $"{redisConf.GetSection("RedisConnectionString").Value},password={redisConf.GetSection("RedisPassword").Value}";
            Console.WriteLine(redCS);
            heals.AddRedis(redCS, 
                redCS,
                timeout: new TimeSpan(0,0,5),
                failureStatus: HealthStatus.Degraded);

            services.AddSingleton<IRedisClientsManagerAsync>(c => new RedisManagerPool());
            services.AddOptions<MongoConf>().Bind(Configuration?.GetSection("MongoSection"));
            services.AddOptions<RedisConf>().Bind(Configuration?.GetSection("RedisSection"));
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHealthChecks("/");

            app.UseHealthChecks("/health", new HealthCheckOptions
            {
                Predicate = _ => true,
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
